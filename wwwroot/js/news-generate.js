(() => {
    const headingInput = document.getElementById('heading');
    const contentInput = document.getElementById('content');
    const contentEditorEl = document.getElementById('content-editor');
    const slugInput = document.getElementById('slug');
    const coverInput = document.getElementById('coverImageUrl');
    const statusEl = document.getElementById('generation-status');
    const thinkingPanel = document.getElementById('thinking-panel');
    const coverPreview = document.getElementById('cover-preview');
    const coverPreviewEmpty = document.getElementById('cover-preview-empty');
    const publishBtn = document.getElementById('btn-publish');
    const saveForm = document.getElementById('news-save-form');

    const btnGenerateHeading = document.getElementById('btn-generate-heading');
    const btnGenerateContent = document.getElementById('btn-generate-content');
    const btnGenerateImage = document.getElementById('btn-generate-image');
    const btnGenerateAll = document.getElementById('btn-generate-all');
    const headingInstruction = document.getElementById('heading-instruction');
    const contentInstruction = document.getElementById('content-instruction');
    const btnReviseHeading = document.getElementById('btn-revise-heading');
    const btnReviseContent = document.getElementById('btn-revise-content');
    const presetButtons = document.querySelectorAll('.conversation-preset');
    const antiforgeryToken = document.querySelector('input[name="__RequestVerificationToken"]')?.value ?? '';

    const quill = new Quill('#content-editor', {
        theme: 'snow',
        modules: {
            toolbar: [
                [{ header: [2, 3, false] }],
                ['bold', 'italic', 'underline'],
                [{ list: 'ordered' }, { list: 'bullet' }],
                ['blockquote', 'link'],
                ['clean']
            ]
        },
        placeholder: 'Generated article content will appear here…'
    });

    if (contentInput.value.trim()) {
        quill.clipboard.dangerouslyPasteHTML(contentInput.value);
    }

    let activeController = null;
    let isGenerating = false;

    function setBusy(busy) {
        isGenerating = busy;
        publishBtn.disabled = busy;
        btnGenerateHeading.disabled = busy;
        btnGenerateContent.disabled = busy;
        btnGenerateImage.disabled = busy;
        btnGenerateAll.disabled = busy;
        btnReviseHeading.disabled = busy;
        btnReviseContent.disabled = busy;
        presetButtons.forEach(button => button.disabled = busy);
        quill.enable(!busy);
    }

    function setStatus(message) {
        statusEl.textContent = message;
    }

    function resetThinking() {
        thinkingPanel.textContent = '';
    }

    function appendThinking(text) {
        if (!text) return;
        thinkingPanel.textContent += text;
        thinkingPanel.scrollTop = thinkingPanel.scrollHeight;
    }

    function syncEditorToInput() {
        const html = quill.root.innerHTML.trim();
        contentInput.value = html === '<p><br></p>' ? '' : html;
    }

    function setEditorHtml(html) {
        quill.setContents([]);
        if (html) {
            quill.clipboard.dangerouslyPasteHTML(html);
        }
        syncEditorToInput();
    }

    function slugify(text) {
        return text
            .toLowerCase()
            .replace(/[^a-z0-9\s-]/g, '')
            .trim()
            .replace(/\s+/g, '-')
            .replace(/-+/g, '-')
            .slice(0, 80)
            .replace(/-+$/, '');
    }

    function updateCoverPreview() {
        const url = coverInput.value.trim();
        if (!url) {
            coverPreview.classList.add('d-none');
            coverPreview.removeAttribute('src');
            coverPreviewEmpty.classList.remove('d-none');
            return;
        }

        coverPreview.src = url;
        coverPreview.classList.remove('d-none');
        coverPreviewEmpty.classList.add('d-none');
    }

    function buildStreamUrl(field, heading) {
        const params = new URLSearchParams({ field });
        if (heading) {
            params.set('heading', heading);
        }
        return `/News/Stream?${params.toString()}`;
    }

    async function openConversationStream(target, instruction) {
        if (activeController) {
            activeController.abort();
        }

        syncEditorToInput();
        activeController = new AbortController();
        resetThinking();

        const response = await fetch('/News/StreamConversation', {
            method: 'POST',
            signal: activeController.signal,
            credentials: 'same-origin',
            headers: {
                Accept: 'text/event-stream',
                'Content-Type': 'application/json',
                RequestVerificationToken: antiforgeryToken
            },
            body: JSON.stringify({
                target,
                instruction,
                heading: headingInput.value.trim(),
                content: contentInput.value.trim()
            })
        });

        if (response.status === 401) {
            throw new Error('You must be signed in to revise content.');
        }

        if (!response.ok) {
            throw new Error(`Conversation failed (${response.status})`);
        }

        return response.body.getReader();
    }

    async function readSseStream(reader, onContent) {
        const decoder = new TextDecoder();
        let buffer = '';
        let accumulated = '';

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

                if (payload.thinking) {
                    appendThinking(payload.thinking);
                }

                if (payload.content) {
                    accumulated += payload.content;
                    onContent(accumulated);
                }
            }
        }

        return accumulated.trim();
    }

    async function streamToInput(field, targetInput, heading) {
        if (activeController) {
            activeController.abort();
        }

        activeController = new AbortController();
        targetInput.value = '';
        resetThinking();

        const response = await fetch(buildStreamUrl(field, heading), {
            signal: activeController.signal,
            credentials: 'same-origin',
            headers: { Accept: 'text/event-stream' }
        });

        if (response.status === 401) {
            throw new Error('You must be signed in to generate content.');
        }

        if (!response.ok) {
            throw new Error(`Stream failed (${response.status})`);
        }

        const reader = response.body.getReader();
        const decoder = new TextDecoder();
        let buffer = '';
        let accumulated = '';

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

                if (payload.thinking) {
                    appendThinking(payload.thinking);
                }

                if (payload.content) {
                    accumulated += payload.content;
                    targetInput.value = accumulated;
                }
            }
        }

        return accumulated.trim();
    }

    async function streamToEditor(field, heading) {
        if (activeController) {
            activeController.abort();
        }

        activeController = new AbortController();
        setEditorHtml('');
        resetThinking();

        const response = await fetch(buildStreamUrl(field, heading), {
            signal: activeController.signal,
            credentials: 'same-origin',
            headers: { Accept: 'text/event-stream' }
        });

        if (response.status === 401) {
            throw new Error('You must be signed in to generate content.');
        }

        if (!response.ok) {
            throw new Error(`Stream failed (${response.status})`);
        }

        const reader = response.body.getReader();
        const decoder = new TextDecoder();
        let buffer = '';
        let accumulated = '';

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

                if (payload.thinking) {
                    appendThinking(payload.thinking);
                }

                if (payload.content) {
                    accumulated += payload.content;
                    quill.clipboard.dangerouslyPasteHTML(accumulated);
                    syncEditorToInput();
                }
            }
        }

        return accumulated.trim();
    }

    async function generateHeading(manageBusy = true) {
        if (manageBusy) setBusy(true);
        setStatus('Generating heading…');
        try {
            const heading = await streamToInput('heading', headingInput);
            headingInput.value = heading.replace(/^["'`]+|["'`]+$/g, '');
            if (!slugInput.value.trim()) {
                slugInput.placeholder = slugify(headingInput.value) || 'Auto-generated on publish';
            }
            setStatus('Heading generated.');
            return headingInput.value.trim();
        } catch (error) {
            if (error.name !== 'AbortError') {
                setStatus(error.message || 'Heading generation failed.');
            }
            return '';
        } finally {
            if (manageBusy) {
                setBusy(false);
                activeController = null;
            }
        }
    }

    async function generateContent(manageBusy = true) {
        const heading = headingInput.value.trim();
        if (!heading) {
            setStatus('Enter or generate a heading first.');
            return false;
        }

        if (manageBusy) setBusy(true);
        setStatus('Generating article content…');
        try {
            const html = await streamToEditor('content', heading);
            setStatus('Content generated.');
            return html.length > 0;
        } catch (error) {
            if (error.name !== 'AbortError') {
                setStatus(error.message || 'Content generation failed.');
            }
            return false;
        } finally {
            if (manageBusy) {
                setBusy(false);
                activeController = null;
            }
        }
    }

    async function reviseHeading(instruction, manageBusy = true) {
        const heading = headingInput.value.trim();
        if (!heading) {
            setStatus('Generate or enter a heading before asking for revisions.');
            return false;
        }

        if (!instruction.trim()) {
            setStatus('Enter an instruction for the heading revision.');
            return false;
        }

        if (manageBusy) setBusy(true);
        setStatus('Revising heading…');
        try {
            const reader = await openConversationStream('heading', instruction.trim());
            const revisedHeading = await readSseStream(reader, value => {
                headingInput.value = value;
            });

            headingInput.value = revisedHeading.replace(/^["'`]+|["'`]+$/g, '');
            if (!slugInput.value.trim()) {
                slugInput.placeholder = slugify(headingInput.value) || 'Auto-generated on publish';
            }
            headingInstruction.value = '';
            setStatus('Heading revised.');
            return true;
        } catch (error) {
            if (error.name !== 'AbortError') {
                setStatus(error.message || 'Heading revision failed.');
            }
            return false;
        } finally {
            if (manageBusy) {
                setBusy(false);
                activeController = null;
            }
        }
    }

    async function reviseContent(instruction, manageBusy = true) {
        const heading = headingInput.value.trim();
        syncEditorToInput();
        const content = contentInput.value.trim();

        if (!heading) {
            setStatus('Enter or generate a heading before revising the body.');
            return false;
        }

        if (!content) {
            setStatus('Generate or enter article content before asking for revisions.');
            return false;
        }

        if (!instruction.trim()) {
            setStatus('Enter an instruction for the body revision.');
            return false;
        }

        if (manageBusy) setBusy(true);
        setStatus('Revising article body…');
        try {
            const reader = await openConversationStream('content', instruction.trim());
            const revisedHtml = await readSseStream(reader, value => {
                quill.clipboard.dangerouslyPasteHTML(value);
                syncEditorToInput();
            });

            setEditorHtml(revisedHtml);
            contentInstruction.value = '';
            setStatus('Article body revised.');
            return true;
        } catch (error) {
            if (error.name !== 'AbortError') {
                setStatus(error.message || 'Article revision failed.');
            }
            return false;
        } finally {
            if (manageBusy) {
                setBusy(false);
                activeController = null;
            }
        }
    }

    async function generateImageKeyword(manageBusy = true) {
        const heading = headingInput.value.trim();
        if (!heading) {
            setStatus('Enter or generate a heading first.');
            return false;
        }

        if (manageBusy) setBusy(true);
        setStatus('Suggesting cover image keyword…');
        const keywordField = document.createElement('input');
        keywordField.type = 'hidden';

        try {
            const keywordRaw = await streamToInput('keyword', keywordField, heading);
            const keyword = keywordRaw.toLowerCase().replace(/[^a-z0-9-]/g, '') || 'technology';
            coverInput.value = `https://loremflickr.com/800/600/${encodeURIComponent(keyword)}`;
            updateCoverPreview();
            setStatus('Cover image URL updated.');
            return true;
        } catch (error) {
            if (error.name !== 'AbortError') {
                setStatus(error.message || 'Image keyword generation failed.');
            }
            return false;
        } finally {
            if (manageBusy) {
                setBusy(false);
                activeController = null;
            }
        }
    }

    async function generateAll() {
        setBusy(true);
        try {
            const heading = await generateHeading(false);
            if (!heading) return;

            const hasContent = await generateContent(false);
            if (!hasContent) return;

            await generateImageKeyword(false);
            setStatus('All fields generated. Review and publish when ready.');
        } finally {
            setBusy(false);
            activeController = null;
        }
    }

    btnGenerateHeading.addEventListener('click', () => {
        if (!isGenerating) generateHeading();
    });

    btnGenerateContent.addEventListener('click', () => {
        if (!isGenerating) generateContent();
    });

    btnGenerateImage.addEventListener('click', () => {
        if (!isGenerating) generateImageKeyword();
    });

    btnGenerateAll.addEventListener('click', () => {
        if (!isGenerating) generateAll();
    });

    btnReviseHeading.addEventListener('click', () => {
        if (!isGenerating) reviseHeading(headingInstruction.value);
    });

    btnReviseContent.addEventListener('click', () => {
        if (!isGenerating) reviseContent(contentInstruction.value);
    });

    presetButtons.forEach(button => {
        button.addEventListener('click', () => {
            if (isGenerating) return;

            const instruction = button.dataset.instruction ?? '';
            if (button.dataset.target === 'heading') {
                headingInstruction.value = instruction;
                reviseHeading(instruction);
                return;
            }

            contentInstruction.value = instruction;
            reviseContent(instruction);
        });
    });

    coverInput.addEventListener('input', updateCoverPreview);
    headingInput.addEventListener('blur', () => {
        if (!slugInput.value.trim() && headingInput.value.trim()) {
            slugInput.placeholder = slugify(headingInput.value.trim()) || 'Auto-generated on publish';
        }
    });

    quill.on('text-change', syncEditorToInput);

    saveForm.addEventListener('submit', (event) => {
        syncEditorToInput();
        if (!contentInput.value.trim()) {
            event.preventDefault();
            setStatus('Article content is required before publishing.');
        }
    });

    updateCoverPreview();
})();
