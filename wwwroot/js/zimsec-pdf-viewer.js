(function () {
    const root = document.querySelector('.zimsec-pdf-viewer');
    if (!root || typeof pdfjsLib === 'undefined') return;

    const pdfUrl = root.dataset.pdfUrl;
    const pagesEl = root.querySelector('.zimsec-pdf-pages');
    const statusEl = root.querySelector('.zimsec-pdf-status');
    const fallbackEl = root.querySelector('.zimsec-pdf-fallback');
    const prevBtn = root.querySelector('.zimsec-pdf-prev');
    const nextBtn = root.querySelector('.zimsec-pdf-next');
    const indicatorEl = root.querySelector('.zimsec-pdf-page-indicator');

    if (!pdfUrl || !pagesEl) return;

    pdfjsLib.GlobalWorkerOptions.workerSrc = '/lib/pdfjs/pdf.worker.min.js';

    let pdfDoc = null;
    let totalPages = 0;
    let currentPage = 1;
    const rendered = new Map();
    const rendering = new Map();

    function showFallback(message) {
        if (statusEl) statusEl.textContent = message || '';
        pagesEl.classList.add('d-none');
        fallbackEl?.classList.remove('d-none');
    }

    function updateToolbar() {
        if (!pdfDoc) return;
        if (indicatorEl) indicatorEl.textContent = 'Page ' + currentPage + ' of ' + totalPages;
        if (prevBtn) prevBtn.disabled = currentPage <= 1;
        if (nextBtn) nextBtn.disabled = currentPage >= totalPages;
    }

    function pageWidth() {
        return Math.max(280, pagesEl.clientWidth - 16);
    }

    async function renderPage(pageNum, canvas) {
        if (rendered.get(pageNum) === canvas.width) return;
        if (rendering.get(pageNum)) return rendering.get(pageNum);

        const task = (async function () {
            const page = await pdfDoc.getPage(pageNum);
            const baseViewport = page.getViewport({ scale: 1 });
            const scale = pageWidth() / baseViewport.width;
            const viewport = page.getViewport({ scale: scale * (window.devicePixelRatio || 1) });
            const context = canvas.getContext('2d');

            canvas.width = viewport.width;
            canvas.height = viewport.height;
            canvas.style.width = Math.floor(viewport.width / (window.devicePixelRatio || 1)) + 'px';
            canvas.style.height = Math.floor(viewport.height / (window.devicePixelRatio || 1)) + 'px';

            await page.render({ canvasContext: context, viewport: viewport }).promise;
            rendered.set(pageNum, canvas.width);
        })();

        rendering.set(pageNum, task);
        try {
            await task;
        } finally {
            rendering.delete(pageNum);
        }
    }

    function scrollToPage(pageNum) {
        const target = pagesEl.querySelector('[data-page="' + pageNum + '"]');
        if (target) {
            target.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
        currentPage = pageNum;
        updateToolbar();
    }

    function observePages() {
        if (!('IntersectionObserver' in window)) return;

        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (!entry.isIntersecting) return;
                const wrap = entry.target;
                const pageNum = parseInt(wrap.dataset.page, 10);
                const canvas = wrap.querySelector('canvas');
                if (canvas) renderPage(pageNum, canvas);
                if (entry.intersectionRatio > 0.4) {
                    currentPage = pageNum;
                    updateToolbar();
                }
            });
        }, { root: pagesEl, threshold: [0.2, 0.4, 0.6] });

        pagesEl.querySelectorAll('.zimsec-pdf-page').forEach(function (el) {
            observer.observe(el);
        });
    }

    function buildPageShells(count) {
        pagesEl.innerHTML = '';
        for (let i = 1; i <= count; i++) {
            const wrap = document.createElement('div');
            wrap.className = 'zimsec-pdf-page';
            wrap.dataset.page = String(i);
            const label = document.createElement('div');
            label.className = 'zimsec-pdf-page-label';
            label.textContent = 'Page ' + i;
            const canvas = document.createElement('canvas');
            wrap.appendChild(label);
            wrap.appendChild(canvas);
            pagesEl.appendChild(wrap);
        }
    }

    prevBtn?.addEventListener('click', function () {
        if (currentPage > 1) scrollToPage(currentPage - 1);
    });

    nextBtn?.addEventListener('click', function () {
        if (currentPage < totalPages) scrollToPage(currentPage + 1);
    });

    let resizeTimer = null;
    window.addEventListener('resize', function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            rendered.clear();
            pagesEl.querySelectorAll('.zimsec-pdf-page canvas').forEach(function (canvas) {
                const pageNum = parseInt(canvas.closest('.zimsec-pdf-page')?.dataset.page || '0', 10);
                if (pageNum) renderPage(pageNum, canvas);
            });
        }, 200);
    });

    pdfjsLib.getDocument({ url: pdfUrl, withCredentials: true }).promise
        .then(function (pdf) {
            pdfDoc = pdf;
            totalPages = pdf.numPages;
            buildPageShells(totalPages);
            if (statusEl) statusEl.classList.add('d-none');
            updateToolbar();
            observePages();
            scrollToPage(1);
        })
        .catch(function () {
            showFallback('Could not load preview.');
        });
})();
