function setModalMaxHeight(element) {
    this.$element = $(element);
    this.$content = this.$element.find('.modal-content');
    var borderWidth = this.$content.outerHeight() - this.$content.innerHeight();
    var dialogMargin = $(window).width() < 768 ? 20 : 60;
    var contentHeight = $(window).height() - (dialogMargin + borderWidth);
    var headerHeight = this.$element.find('.modal-header').outerHeight() || 0;
    var footerHeight = this.$element.find('.modal-footer').outerHeight() || 0;
    var maxHeight = contentHeight - (headerHeight + footerHeight);

    this.$content.css({
        'overflow': 'hidden'
    });

    this.$element
      .find('.modal-body').css({
          'max-height': maxHeight,
          'overflow-y': 'auto'
      });
}

var modals = [];
$(window).on("resize", function () {
    for (var i = 0; i < modals.length; i++) {
        $('#' + modals[i] + ':visible').each(function () { setModalMaxHeight(this); });
    }
});

function addModal(modal) {
    var exist = false;
    for (var i = 0; i < modals.length; i++) {
        if (modals[i] == modal) {
            exist = true;
            break;
        }
    }
    if (!exist) {
        $('#' + modal).on('shown.bs.modal', function () { setModalMaxHeight(this); });
        modals.push(modal);
    }
}