
/* VARIAVEIS DE USO GLOBAL DA APLICAÇÃO
--------------------------------------------------------------*/
var blockUIMessage = "<div class='ls-alert-info splash'>"
blockUIMessage += "<dl>";
blockUIMessage += "   <dt>Aguarde!</dt>";
blockUIMessage += "   <dt><div class='progress progress-striped active'><div class='bar' style='width: 100%;'></div></div>";
blockUIMessage += "</dl>";
blockUIMessage += "</div>";
var blockUISettings = { title: '', centerY: 15, theme: true, showOverlay: true, message: blockUIMessage };

$(document).ready(function () {

    /* CONFIGURAÇÕES GLOBAIS
    --------------------------------------------------------------*/
    $('a[href=#]').bind('click', function (event) { event.preventDefault(); });
    // BUG FIX : Utilizado para resolver problemas de incompatibilidade do Safari com Touch Bootstrap
    $('body').on('touchstart.dropdown', '.dropdown-menu', function (e) { e.stopPropagation(); });

    $.ajaxSetup({
        cache: false,
        error: function (xhr, exception) {
            var message;
            var statusErrorMap = {
                '400': "Server understood the request but request content was invalid.",
                '401': "Unauthorised access.",
                '403': "Forbidden resouce can't be accessed",
                '404': "File or resource not found.",
                '500': "Internal Server Error.",
                '503': "Service Unavailable"
            };

            if (xhr.status) {
                message = statusErrorMap[xhr.status.toString()];
                if (!message) { message = "Unknow Error."; }
            }
            else if (exception == 'parsererror') { message = "Error.\nParsing JSON Request failed."; }
            else if (exception == 'timeout') { message = "Request Time out."; }
            else if (exception == 'abort') { message = "Request was aborted by the server"; }
            else { message = "Unknow Error."; }

            Exception.show(message, '$.ajaxSetup.error()');
        }
    });


    /* CONFIGURAÇÃO DA CLASS ALERT (BOOTSTRAP)
    -------------------------------------------------------------*/
    setTimeout(function () {
        $("div.ls-alert-info:not(:has(a.close, .splash, ul#validation-summary-list))").fadeOut(500, function () { $(this).remove(); });
    }, 7000);


    /* ATRIBUIÇÃO DE SELETORES GLOBAIS
    -------------------------------------------------------------*/
    $('input:text.selectall-in-focus').focus(function () { $(this).select(); });
    $('input:text.selectall-in-focus').mouseup(function (e) { e.preventDefault(); });

    $('div:has(div#accessDeniedSection, a#voltarAcessoNegadoButton) > a#voltarAcessoNegadoButton').bind('click', function () { history.go(-1) });


    /*
    // Exemplo para tratamento de erros registrando no ajax error
    $('*').ajaxError(function(event, xhr, settings, exception){        
        var message;
        var statusErrorMap = {
            '400' : "Server understood the request but request content was invalid.",
            '401' : "Unauthorised access.",
            '403' : "Forbidden resouce can't be accessed",
            '404' : "File or resource not found.",
            '500' : "Internal Server Error.",
            '503' : "Service Unavailable"
        };
                    
        if (xhr.status) { 
            message = statusErrorMap[xhr.status.toString()]; 
            if(!message){ message = "Unknow Error."; } 
        } 
        else if(event == 'parsererror'  )   { message = "Error.\nParsing JSON Request failed."; } 
        else if(event == 'timeout'      )   { message = "Request Time out."; }
        else if(event == 'abort'        )   { message = "Request was aborted by the server"; } 
        else                                { message = "Unknow Error."; }
                        
        Exception.show(message);
        
        return false;        
    });
    */


    // Formatacao Global

    $('.date-mes-ano').mask('M#/2Z##', {
        'translation': {
            M: { pattern: /[0-1]/ },
            V: { pattern: /'20'[0-99]/ },
            Z: { pattern: /0/ }
        }
    });

});


/* GLOBAL FUNCTIONS
--------------------------------------------------------------*/
// Objeto responsável por exibir a modal de tratamento de exceções de validação
ValidationSummaryPlugin = function ()
{ this.init(); }

$.extend(ValidationSummaryPlugin.prototype, {
    _instance: this,
    _element: 'div#modal-validation-summary',
    _title: 'Resultado da validação!',
    _subTitle: 'Existem dados inválidos que impedem a conclusão do processo. Por gentileza, efetue as devidas correções e execute o processo novamente.',
    init: function () {


        ////locastyle.modal.open({ target: this._element });


        ////$(this._element).modal({
        //////locastyle.modal.open({
        //    backdrop: true,
        //    keyboard: true,
        //    show: false,
        //    remote: false
        //})
        //.on('show', function () { })
        //.on('hide', function () { });
    },
    show: function (exception, method) {
        if (exception == null) { return; }

        $(this._element).find('div.ls-modal-header h3#ls-modal-title').text(this._title);
        $(this._element).find('div.ls-modal-body   p#summary-subtitle').text(this._subTitle);
        $(this._element).find('div.ls-modal-body   div#validation-summary-container ul').empty();

        this.renderException(exception, method);

        $.unblockUI();
        ////locastyle.modal.open({ target: this._element });
        //$(this._element).modal('show');
    },
    close: function () {
        ////locastyle.modal.close({ target: this._element });
        //$(this._element).modal('hide');
    },
    renderException: function (exception, method) {
        var target = $(this._element).find('div.ls-modal-body div#validation-summary-container ul');

        if (exception == null) { return; }

        $.each(exception, function (index, item) {
            $('<li>')
                .text(item)
            .appendTo(target);
        });
    }
});
// Instância default do ValidationSummaryPlugin
var ValidationSummary = new ValidationSummaryPlugin();


// Objeto responsável por exibir a modal de tratamento de exceções
ExceptionPlugin = function (_element, _title, _subTitle)
{ this.init(_element, _title, _subTitle); }

$.extend(ExceptionPlugin.prototype, {
    _instance: this,
    _element: 'div#modal-exceptions',
    _title: 'Oops! Ocorreu um erro...',
    _subTitle: 'Ocorreu um erro durante o processamento da requisição. Para continuar, atualize ou vá para outra página.',
    init: function (element, title, subTitle) {
        this._element = (element == null ? this._element : element);
        this._title = (title == null ? this._title : title);
        this._subTitle = (subTitle == null ? this._subTitle : subTitle);

        ////locastyle.modal.open({ target: this._element });
        //$(this._element).modal({
        //    backdrop: true,
        //    keyboard: true,
        //    show: false,
        //    remote: false
        //})
        //.on('show', function () { })
        //.on('hide', function () { });
    },
    show: function (exception, method) {
        if (exception == null) { return; }

        $(this._element).find('div.ls-modal-header h3#exception-title').text(this._title);
        $(this._element).find('div.ls-modal-body   p#exception-subtitle').text(this._subTitle);
        $(this._element).find('div.ls-modal-body   table#exceptionsTable tbody').empty();

        this.renderException(exception, method);

        $.unblockUI();
        ////locastyle.modal.open({ target: this._element });
        //$(this._element).modal('show');
    },
    close: function () {
        ////locastyle.modal.close({ target: this._element });
        //$(this._element).modal('hide');
    },
    renderException: function (exception, method) {
        var target = $(this._element).find('div.ls-modal-body table#exceptionsTable tbody');

        if (exception == null) { return; }
        if (!method) { method = new String(); }
        var isArray = (typeof (exception) === 'object' && exception.constructor.toString().indexOf("Array") > -1);

        method = (method.isNullOrEmpty() ? '[não definido]' : method);

        if (isArray == false)
            $('<tr>').addClass('error')
                .append($('<td>').addClass('span9').text(exception))
                .append($('<td>').addClass('span3').text(method))
            .appendTo(target);
        else
            $.each(exception, function (index, item) {
                $('<tr>').addClass('error')
                .append($('<td>').addClass('span9 wrap-content').text(item))
                .append($('<td>').addClass('span3 wrap-content').text(method))
                .appendTo(target);
            });

    }
});
// Instância default do ExceptionPlugin
var Exception = new ExceptionPlugin(null, null, null);

function TrocarServicoAtual(idServico) {
    var methodName = GetMethodName(arguments.callee);

    try {

        //alert(window.location.host);
        //if (idServico == 0) { return; }
        //var _host = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ":" + window.location.port : ""); var b = window.location.pathname.lastIndexOf("/");

        $.ajax({
            url: ('http://' + window.location.host + '/Base/AjaxCallTrocarServicoAtual'),
            dataType: 'json',
            type: 'POST',
            data: {
                idServico: parseInt(idServico)
            },
            success: function (data, textstatus, xhr) {
                if (data == null) { return; }
                if (data.success == true) {
                    if (parseInt(idServico) == 0) {
                        return true;
                    } else {
                        window.location = 'http://' + window.location.host + '/home';
                    }
                    
                }
                //else { Exception.show(methodName); }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}