
    function initMenuToggle() {
    // Side Menu Toggle
    document.querySelector('.side-menu-toggle').addEventListener('click', function() {
        document.querySelector('.side-menu').classList.toggle('side-menu-open');
    });

    // Top Right Menu Toggle
    document.querySelector('.top-right-menu-toggle').addEventListener('click', function() {
    document.querySelector('.top-right-menu').classList.toggle('top-right-menu-open');
});
    }
