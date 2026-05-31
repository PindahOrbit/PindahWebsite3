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

    function escapeHtml(text) {
        return text
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;');
    }

    function formatAssistantText(text) {
        const cleaned = text
            .replace(/```whatsapp-handoff[\s\S]*?```/gi, '')
            .trim();

        return escapeHtml(cleaned)
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
                        assistantBubble.innerHTML = formatAssistantText(accumulated);
                        messagesEl.scrollTop = messagesEl.scrollHeight;
                    }
                }
            }

            const finalText = accumulated.trim();
            if (finalText) {
                conversation.push({ role: 'assistant', content: finalText });
                assistantBubble.innerHTML = formatAssistantText(finalText);

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
