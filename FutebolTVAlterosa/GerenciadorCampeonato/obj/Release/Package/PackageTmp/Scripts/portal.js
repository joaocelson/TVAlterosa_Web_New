

function error_handler(e) {
    if (e.errors) {
        var message = "";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });

        showErrorMessage(message);
    }
}

function sync_handler(e) {    
    this.read();
}

function showErrorMessage(message) {
    $("#errorBox").show();
    $("#errorMessage").html(message);
}

function hideErrorMessage() {
    $("#errorBox").hide();
    $("#errorMessage").html("");
}

function showSuccessMessage(message) {
    $("#successBox").show();
    $("#successMessage").html(message);
}

function hideSuccessMessage() {
    $("#successBox").hide();
    $("#successMessage").html("");
}

function onRequestEnd(e)
{
    if (((e.type == "create") || (e.type == "update") || (e.type == "destroy")) && (e.response.Errors == null))
    {
        var operacao = "";

        switch (e.type) {
            case "create": operacao = "Inserção"; break;
            case "update": operacao = "Atualização"; break;
            case "destroy": operacao = "Exclusão"; break;
            default: break;
        }

        showSuccessMessage(operacao + " realizada com sucesso.");
    }
}

$(function () {
    //$("#gridAllFilterReset").click(function (e) {
    //    e.preventDefault();
    //    $("form.k-filter-menu button[type='reset']").trigger("click");
    //});

    // Controla visualização do menu
    var path = window.location.pathname;
    var currentAction = $(".page-sidebar-menu a[href='" + path + "']");
    $(currentAction).closest("li").addClass("active");
    $(currentAction).closest(".sub-menu").css("display", "block");
    $(currentAction).closest(".page-sidebar-menu > li").addClass("open active");

    $('.k-grid-content').on('click', 'a.k-grid-edit', function () {
        hideErrorMessage();
        hideSuccessMessage();
    });

    $('.k-grid-content').on('click', 'a.k-grid-delete', function () {
        hideErrorMessage();
        hideSuccessMessage();
    });

    $('.k-grid').on('click', 'a.k-grid-add', function () {
        hideErrorMessage();
        hideSuccessMessage();
    });

    $(".k-widget.k-tooltip.k-tooltip-validation.k-invalid-msg").each(function () {
        $(this).prepend("<span class='k-icon k-warning'> </span>");
    });
});


function CollapseFormPesquisa(botaoCabecalho, divForm) {
    botaoCabecalho.removeClass("collapse");
    botaoCabecalho.addClass("expand");
    divForm.css("display", "none");
}

function ExpandFormPesquisa(botaoCabecalho, divForm) {
    botaoCabecalho.removeClass("expand");
    botaoCabecalho.addClass("collapse");
    divForm.css("display", "block");
}

function AjustaTelaPesquisa(divForm) {
    $('html, body').animate({ 'scrollTop': divForm.height() + 200 }, 1);
}

function AjustaTelaPesquisaErro(divForm) {
    $('html, body').animate({ 'scrollTop': 0 }, 1);
}


function ToolTip(target, tooltip, closeElementClick) {

    tooltip.css('display', 'block');

    var init_tooltip = function () {
        if ($(window).width() < tooltip.outerWidth() * 1.5)
            tooltip.css('max-width', $(window).width() - 10); // /2
        else
            tooltip.css('max-width', 340);

        var pos_left = ($(window).width() - (tooltip.outerWidth())) / 2,
            pos_top = target.offset().top - tooltip.outerHeight() - 20;

        //var pos_left = target.offset().left + (target.outerWidth() / 2) - (tooltip.outerWidth() / 2),
        //    pos_top = target.offset().top - tooltip.outerHeight() - 20;

        if (pos_left < 0) {
            pos_left = target.offset().left + target.outerWidth() / 2 - 20;
            tooltip.addClass('left');
        }
        else
            tooltip.removeClass('left');

        if (pos_left + tooltip.outerWidth() > $(window).width()) {
            pos_left = target.offset().left - tooltip.outerWidth() + target.outerWidth() / 2 + 20;
            tooltip.addClass('right');
        }
        else
            tooltip.removeClass('right');

        if (pos_top < 0) {
            var pos_top = target.offset().top + target.outerHeight();
            tooltip.addClass('top');
            //pos_top -= 100;
        }
        else {
            tooltip.removeClass('top');
            //pos_top += 100;
        }

        tooltip.css({ left: pos_left, top: pos_top })
               .animate({ top: '+=10', opacity: 1 }, 50);
    };

    init_tooltip();
    $(window).resize(init_tooltip);

    if (closeElementClick == null)
        closeElementClick = tooltip;

    var remove_tooltip = function () {
        tooltip.animate({ top: '-=10', opacity: 0 }, 50, function () {
            //$(this).remove();
            $(this).css('display', 'none');
        });
    };

    //target.bind('mouseleave', remove_tooltip);
    closeElementClick.bind('click', remove_tooltip);
}

function removeKendoElementFromDOM(kendoElement, htmlElement) {
    if ((kendoElement != null) && !(kendoElement === undefined))
        kendoElement.destroy();

    htmlElement.remove();
}

function truncateDecimals(num, digits) {
    var numS = num.toString(),
        decPos = numS.indexOf('.'),
        substrLength = decPos == -1 ? numS.length : 1 + decPos + digits,
        trimmedResult = numS.substr(0, substrLength),
        finalResult = isNaN(trimmedResult) ? 0 : trimmedResult;

    return parseFloat(finalResult);
}

function currencyFormatDE(num) {
    return num
       .toFixed(2) // always two decimal digits
       .replace(".", ",") // replace decimal point character with ,
       .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") // use . as a separator
}

function numberWithThousandSeparator(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    return parts.join(".");
}

$(window).on('unload', function () {

    var pageId = typeof $('#hddPageId').val() == 'undefined' ? -1 : $('#hddPageId').val();

    if ((pageId != '') && (pageId != -1)) {

        var _data = { pageId: pageId };

        $.ajax({
            type: "POST",
            url: "/Session/ClearSessionById",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(_data)
        });
    }
});

$.ajaxSetup({
    statusCode: {
        401: function () {
            window.location.href = "/Autenticacao/LogIn";
        },
        500: function (xhr) {
            try {
                var json = $.parseJSON(xhr.responseText);                
            } catch (e) {
                window.location.href = "/Home/Error";
            }
        }
    }
});

$(window).on("resize", function () {
    kendo.resize($(".chart-wrapper"));
});

$.extend(window.kendo.ui.NumericTextBox.fn, {
    clear: function () {
        this._old = this._value;
        this._value = null;
        this._text.val(this._value);
        this.element.val(this._value);
    }
});