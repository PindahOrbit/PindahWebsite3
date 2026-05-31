(() => {
    const messagesEl = document.getElementById('sales-chat-messages');
    const inputEl = document.getElementById('sales-chat-input');
    const sendBtn = document.getElementById('sales-chat-send');
    const statusEl = document.getElementById('sales-chat-status');
    const handoffPanel = document.getElementById('sales-chat-handoff');
    const handoffSummaryEl = document.getElementById('sales-chat-handoff-summary');
    const whatsappLinkEl = document.getElementById('sales-chat-whatsapp-link');

    if (!messagesEl || !inputEl || !sendBtn) {
        return;
    }

    const conversation = [];
    let activeController = null;
    let isStreaming = false;

    const ALLOWED_TAGS = new Set([
        'P', 'DIV', 'SPAN', 'STRONG', 'EM', 'B', 'I', 'UL', 'OL', 'LI',
        'H5', 'H6', 'SMALL', 'TABLE', 'THEAD', 'TBODY', 'TR', 'TD', 'TH',
        'BR', 'A', 'HR', 'DL', 'DT', 'DD', 'BLOCKQUOTE'
    ]);

    const ALLOWED_CLASSES = /^[\w\s-]+$/;

    function escapeHtml(text) {
        return text
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    const VOID_TAGS = new Set(['AREA', 'BASE', 'BR', 'COL', 'EMBED', 'HR', 'IMG', 'INPUT', 'LINK', 'META', 'PARAM', 'SOURCE', 'TRACK', 'WBR']);

    function cleanVisibleAssistantText(text) {
        return text
            .replace(/```whatsapp-handoff[\s\S]*?```/gi, '')
            .replace(/```whatsapp-handoff[\s\S]*$/i, '')
            .replace(/```[^\n`]*$/i, '')
            .trim();
    }

    function shouldRenderAsHtml(text) {
        return /</.test(text);
    }

    /** Drop a trailing tag that has not finished (e.g. "<p class=\"mb"). */
    function trimIncompleteTrailingTag(html) {
        const incomplete = html.match(/<[^>]*$/);
        if (incomplete) {
            return html.slice(0, -incomplete[0].length);
        }
        return html;
    }

    /** Temporarily close open tags so partial stream HTML still parses. */
    function balanceOpenTags(html) {
        const stack = [];
        const tagRegex = /<\/?([a-zA-Z][a-zA-Z0-9]*)\b[^>]*\/?>/g;
        let match;

        while ((match = tagRegex.exec(html)) !== null) {
            const token = match[0];
            const name = match[1].toUpperCase();

            if (VOID_TAGS.has(name) || token.endsWith('/>')) {
                continue;
            }

            if (token.startsWith('</')) {
                const idx = stack.lastIndexOf(name);
                if (idx >= 0) {
                    stack.splice(idx, 1);
                }
            } else {
                stack.push(name);
            }
        }

        let balanced = html;
        for (let i = stack.length - 1; i >= 0; i -= 1) {
            balanced += `</${stack[i].toLowerCase()}>`;
        }
        return balanced;
    }

    function prepareStreamingHtml(raw) {
        const cleaned = cleanVisibleAssistantText(raw);
        if (!cleaned) {
            return '';
        }

        if (!shouldRenderAsHtml(cleaned)) {
            return cleaned;
        }

        return balanceOpenTags(trimIncompleteTrailingTag(cleaned));
    }

    function sanitizeAssistantHtml(html) {
        const template = document.createElement('template');
        template.innerHTML = html;

        function walk(node) {
            const children = [...node.childNodes];
            for (const child of children) {
                if (child.nodeType === Node.TEXT_NODE) {
                    continue;
                }

                if (child.nodeType !== Node.ELEMENT_NODE) {
                    child.remove();
                    continue;
                }

                const tag = child.tagName;
                if (!ALLOWED_TAGS.has(tag)) {
                    const text = document.createTextNode(child.textContent);
                    child.replaceWith(text);
                    continue;
                }

                [...child.attributes].forEach(attr => {
                    const name = attr.name.toLowerCase();
                    if (name === 'class' && ALLOWED_CLASSES.test(attr.value)) {
                        return;
                    }
                    if (tag === 'A' && name === 'href' && /^(https?:\/\/|mailto:)/i.test(attr.value)) {
                        child.setAttribute('rel', 'noopener noreferrer');
                        child.setAttribute('target', '_blank');
                        return;
                    }
                    child.removeAttribute(attr.name);
                });

                walk(child);
            }
        }

        walk(template.content);
        return template.innerHTML;
    }

    function renderAssistantContent(text) {
        const prepared = prepareStreamingHtml(text);
        if (!prepared) {
            return '';
        }

        if (shouldRenderAsHtml(prepared)) {
            return sanitizeAssistantHtml(prepared);
        }

        return escapeHtml(prepared)
            .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
            .replace(/\n/g, '<br>');
    }

    function appendBubble(role, html, isHtml = false) {
        const bubble = document.createElement('div');
        bubble.className = `sales-chat-bubble sales-chat-bubble-${role}`;
        if (isHtml) {
            bubble.innerHTML = html;
        } else {
            bubble.textContent = html;
        }
        messagesEl.appendChild(bubble);
        messagesEl.scrollTop = messagesEl.scrollHeight;
        return bubble;
    }

    function setBusy(busy) {
        isStreaming = busy;
        sendBtn.disabled = busy;
        inputEl.disabled = busy;
    }

    function setStatus(message) {
        statusEl.textContent = message;
    }

    function parseHandoff(fullText) {
        const match = fullText.match(/```whatsapp-handoff\s*([\s\S]*?)```/i);
        if (!match) {
            return null;
        }

        try {
            return JSON.parse(match[1].trim());
        } catch {
            return null;
        }
    }

    async function buildWhatsAppUrl(handoff) {
        const response = await fetch('/ChatAgent/whatsapp', {
            method: 'POST',
            credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                summary: handoff.summary ?? '',
                features: handoff.features ?? [],
                pricing: handoff.pricing ?? ''
            })
        });

        if (!response.ok) {
            throw new Error('Could not build WhatsApp link.');
        }

        const data = await response.json();
        return data.url;
    }

    async function showHandoff(handoff) {
        handoffPanel.classList.remove('d-none');
        handoffSummaryEl.textContent = handoff.summary ?? 'Summary ready for WhatsApp.';

        try {
            const url = await buildWhatsAppUrl(handoff);
            whatsappLinkEl.href = url;
        } catch {
            whatsappLinkEl.href = 'https://wa.me/263714856897';
        }
    }

    async function sendMessage() {
        const text = inputEl.value.trim();
        if (!text || isStreaming) {
            return;
        }

        inputEl.value = '';
        appendBubble('user', text);
        conversation.push({ role: 'user', content: text });

        if (activeController) {
            activeController.abort();
        }

        activeController = new AbortController();
        setBusy(true);
        setStatus('Thinking…');

        const assistantBubble = appendBubble('assistant', '', true);
        let accumulated = '';

        try {
            const response = await fetch('/ChatAgent/stream', {
                method: 'POST',
                signal: activeController.signal,
                credentials: 'same-origin',
                headers: {
                    Accept: 'text/event-stream',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ messages: conversation })
            });

            if (!response.ok) {
                throw new Error(`Chat failed (${response.status})`);
            }

            const reader = response.body.getReader();
            const decoder = new TextDecoder();
            let buffer = '';

            while (true) {
                const { value, done } = await reader.read();
                if (done) break;

                buffer += decoder.decode(value, { stream: true });
                const parts = buffer.split('\n\n');
                buffer = parts.pop() ?? '';

                for (const part of parts) {
                    const line = part.split('\n').find(l => l.startsWith('data: '));
                    if (!line) continue;

                    let payload;
                    try {
                        payload = JSON.parse(line.slice(6));
                    } catch {
                        continue;
                    }

                    if (payload.error) {
                        throw new Error(payload.error);
                    }

                    if (payload.content) {
                        accumulated += payload.content;
                        assistantBubble.innerHTML = renderAssistantContent(accumulated);
                        messagesEl.scrollTop = messagesEl.scrollHeight;
                    }
                }
            }

            const finalText = accumulated.trim();
            if (finalText) {
                conversation.push({ role: 'assistant', content: finalText });
                assistantBubble.innerHTML = renderAssistantContent(finalText);

                const handoff = parseHandoff(finalText);
                if (handoff) {
                    await showHandoff(handoff);
                    setStatus('Ready — continue on WhatsApp when you are ready.');
                } else {
                    setStatus('Ready');
                }
            } else {
                assistantBubble.textContent = 'Sorry, I could not generate a response. Please try again.';
                setStatus('Ready');
            }
        } catch (error) {
            if (error.name !== 'AbortError') {
                assistantBubble.textContent = error.message || 'Something went wrong. Please try again.';
                setStatus('Ready');
            }
        } finally {
            setBusy(false);
            activeController = null;
        }
    }

    sendBtn.addEventListener('click', sendMessage);

    inputEl.addEventListener('keydown', event => {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            sendMessage();
        }
    });
})();
