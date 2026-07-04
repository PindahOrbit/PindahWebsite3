(function () {
    const root = document.querySelector('.zimsec-library');
    if (!root) return;

    const input = document.getElementById('zimsecSearchInput');
    const suggestions = document.getElementById('zimsecSearchSuggestions');
    const searchUrl = root.dataset.searchUrl;
    if (!input || !suggestions || !searchUrl) return;

    let debounceTimer = null;

    input.addEventListener('input', function () {
        const q = input.value.trim();
        clearTimeout(debounceTimer);
        if (q.length < 2) {
            suggestions.classList.add('d-none');
            suggestions.innerHTML = '';
            return;
        }

        debounceTimer = setTimeout(async function () {
            try {
                const params = new URLSearchParams(window.location.search);
                params.set('q', q);
                const res = await fetch(searchUrl + '?' + params.toString(), {
                    headers: { 'Accept': 'application/json' }
                });
                if (!res.ok) return;
                const data = await res.json();
                if (!data.hits || data.hits.length === 0) {
                    suggestions.classList.add('d-none');
                    return;
                }

                suggestions.innerHTML = data.hits.slice(0, 6).map(function (hit) {
                    return '<a class="zimsec-suggestion-item" href="' + hit.url + '">' +
                        '<div class="zimsec-suggestion-title">' + escapeHtml(hit.title) + '</div>' +
                        '<div class="zimsec-suggestion-meta">' + escapeHtml(hit.level + ' › ' + hit.subject) + '</div>' +
                        (hit.snippet ? '<div class="zimsec-suggestion-meta">' + hit.snippet + '</div>' : '') +
                        '</a>';
                }).join('');
                suggestions.classList.remove('d-none');
            } catch (_) { /* ignore */ }
        }, 280);
    });

    document.addEventListener('click', function (e) {
        if (!suggestions.contains(e.target) && e.target !== input) {
            suggestions.classList.add('d-none');
        }
    });

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
})();
