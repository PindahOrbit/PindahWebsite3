(function () {
    const root = document.querySelector('.zimsec-library');
    if (!root) return;

    const offcanvasEl = document.getElementById('zimsecNav');

    function initSidebarNav(nav) {
        nav.querySelectorAll('.zimsec-nav-group-toggle').forEach(function (btn) {
            btn.addEventListener('click', function () {
                const group = btn.closest('.zimsec-nav-group');
                if (!group) return;
                const open = group.classList.toggle('is-open');
                group.classList.remove('is-filter-open');
                btn.setAttribute('aria-expanded', open ? 'true' : 'false');
            });
        });

        const filterInput = nav.querySelector('.zimsec-nav-filter');
        const noMatch = nav.querySelector('.zimsec-nav-no-match');
        if (!filterInput) return;

        filterInput.addEventListener('input', function () {
            const query = filterInput.value.trim().toLowerCase();
            const groups = nav.querySelectorAll('.zimsec-nav-group');
            let visibleSubjects = 0;

            groups.forEach(function (group) {
                let groupVisible = 0;
                group.querySelectorAll('.zimsec-nav-item[data-subject-name]').forEach(function (link) {
                    const name = (link.getAttribute('data-subject-name') || link.textContent || '').toLowerCase();
                    const match = !query || name.indexOf(query) !== -1;
                    link.classList.toggle('is-filter-hidden', !match);
                    if (match) groupVisible += 1;
                });

                if (!query) {
                    group.classList.remove('is-filter-hidden', 'is-filter-open');
                } else {
                    group.classList.toggle('is-filter-hidden', groupVisible === 0);
                    group.classList.toggle('is-filter-open', groupVisible > 0);
                }

                visibleSubjects += groupVisible;
            });

            if (noMatch) {
                noMatch.classList.toggle('d-none', !query || visibleSubjects > 0);
            }
        });

        nav.querySelectorAll('.zimsec-nav-item[href]').forEach(function (link) {
            link.addEventListener('click', function () {
                if (!offcanvasEl) return;
                const instance = bootstrap.Offcanvas.getInstance(offcanvasEl);
                if (instance) instance.hide();
            });
        });
    }

    root.querySelectorAll('.zimsec-nav').forEach(initSidebarNav);

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
