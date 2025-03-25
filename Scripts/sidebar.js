// sidebar.js
$(document).ready(function () {
    // Toggle sidebar
    $('.toggle-sidebar').on('click', function () {
        $('#sidebar').toggleClass('collapsed');
        $('#content').toggleClass('expanded');
        $('.topbar').toggleClass('expanded');
        $('.sidebar-overlay').toggleClass('active');
    });

    // Close sidebar when clicking on overlay (mobile view)
    $('.sidebar-overlay').on('click', function () {
        $('#sidebar').addClass('collapsed');
        $('.sidebar-overlay').removeClass('active');
    });

    // Handle active menu item
    $('.sidebar-item').on('click', function () {
        $('.sidebar-item').removeClass('active');
        $(this).addClass('active');
    });

    // Adjust for mobile view on page load
    function checkWidth() {
        if ($(window).width() < 768) {
            $('#sidebar').addClass('collapsed');
            $('#content').removeClass('expanded');
            $('.topbar').removeClass('expanded');
        }
    }

    // Initial check and resize handler
    checkWidth();
    $(window).resize(function () {
        checkWidth();
    });
});