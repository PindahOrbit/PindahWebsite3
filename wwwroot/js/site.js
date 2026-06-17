(function () {
    'use strict';

    var currentPath = window.location.pathname.toLowerCase().replace(/\/$/, '') || '/';
    var navLinks = document.querySelectorAll('#mainNav .nav-link');
    for (var i = 0; i < navLinks.length; i++) {
        var href = navLinks[i].getAttribute('href');
        if (href) {
            var linkPath = href.toLowerCase().replace(/\/$/, '') || '/';
            if (linkPath === currentPath) {
                navLinks[i].classList.add('active');
                navLinks[i].setAttribute('aria-current', 'page');
            }
        }
    }
})();
