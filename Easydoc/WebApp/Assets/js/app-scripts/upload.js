jQuery(document).ready(function () { init(); });

var init = function () {
    $('a#btnGerarLote')
        .bind('click', function (event) { event.preventDefault(); GerarLote(); });
    $('a#btnCancelarLote')
        .unbind('click');
    $('a#btnEncerrarLote')
        .unbind('click');
}

var bindControles = function () {

    $('a#btnGerarLote')
        .bind('click', function (event) { event.preventDefault(); GerarLote(); });
    $('a#btnCancelarLote')
        .bind('click', function (event) { event.preventDefault(); CancelarLote(); });
    $('a#btnEncerrarLote')
        .bind('click', function (event) { event.preventDefault(); EncerrarLote(); });

}

function limpamsg() {
    $('#msg').hide();
    $('#msgatencao').hide();
}

var EncerrarLote = function () {
    $('#msg').slideUp(1000);
    $.ajax({
        url: '../Upload/AjaxCallEncerrarLote',
        dataType: 'json',
        type: 'POST',
        traditional: true,
        cache: false,
        async: false,
        beforeSend: function (xhr) { $.blockUI(blockUISettings); },
        success: function (data, textstatus, xmlhttprequest) {
            if (data == null) { return; }

            if (data.success == true) {
                
                exibirmsg('Documento capturado com sucesso.');

                $('a#btnGerarLote')
                    .bind('click', function (event) { event.preventDefault(); GerarLote(); });
                $('a#btnCancelarLote')
                    .unbind('click');
                $('a#btnEncerrarLote')
                    .unbind('click');

                $('div#painel-upload').css('display', 'none');
            }
            
            $.unblockUI();
            $('div#modal-resultado-upload span#texto-resultado').text(data.message);
            ////locastyle.modal.open({ target: '#modal-resultado-upload' });
            return true;
        }
    }).abort();
    
}
var CancelarLote = function () {
    $('#msg').hide();
    $('div#painel-upload').css('display', 'none');
    $.ajax({
        url: '../Upload/AjaxCallCancelarLote',
        dataType: 'json',
        type: 'POST',
        traditional: true,
        beforeSend: function (xhr) { $.blockUI(blockUISettings); },
        success: function (data, textstatus, xmlhttprequest) {
            if (data == null) { $.unblockUI(); return; }

            if (data.success == true) {

                $(".progress-bar").css('width', '0%');
                exibirmsgatencao('Lote cancelado.');

                $('a#btnGerarLote')
                    .bind('click', function (event) { event.preventDefault(); GerarLote(); });
                $('a#btnCancelarLote')
                    .unbind('click');
                $('a#btnEncerrarLote')
                    .unbind('click');
            }
            else { Exception.show(data.message, methodName); }
            $.unblockUI();
        }
        
    });
    $.unblockUI();
}
var GerarLote = function () {
    limpamsg();

    $('a#btnGerarLote')
        .unbind('click');
    $.ajax({
        url: '../Upload/AjaxCallGerarLote',
        dataType: 'json',
        type: 'POST',
        traditional: true,
        cache: false,
        async: false,
        beforeSend: function (xhr) { $.blockUI(blockUISettings); },
        success: function (data, textstatus, xmlhttprequest) {
            if (data == null) { $.unblockUI(); return; }

            $('#numLote').text(data.output.ID);

            if (data.success == true) {
                $('div#painel-upload').css('display', 'block');
                createUploader();
                $('a#btnCancelarLote')
                    .bind('click', function (event) { event.preventDefault(); CancelarLote(); });
            }else { Exception.show(data.message, methodName); }
            $.unblockUI();
            return;
        }
    });
}
var createUploader = function () {
    $('a#btnEncerrarLote')
                .bind('click', function (event) { event.preventDefault(); EncerrarLote(); });
    $(".progress-bar").css('width', '0%');
    var i = 0;
    var uploader = new qq.FileUploader({
        element: document.getElementById('file-uploader'),//jQuery("#file_uploader")[0],
        action: '../Upload/SaveFiles', //@Url.Action("SaveFiles", new { area = "", controller = "Upload" }),
        //action:  @Url.Action("SaveFiles", new { area = "", controller = "Upload" }),
        //action: '@Url.Action("SaveFiles", "Upload")',
        // additional data to send, name-value pass     
        params: { id: $("#id").val() },
        // validation
        // ex. ['jpg', 'jpeg', 'png', 'gif'] or []
        allowedExtensions: ['png', 'jpg', 'jpeg', 'gif', 'bmp', 'pdf','tif','tiff', 'json'],
        uploadButtonText: "Clique para selecionar os arquivos ou arraste para essa area",
        // each file size limit in bytes
        // this option isn't supported in all browsers     
        sizeLimit: 2147483647, // max size
        minSizeLimit: 0, // min size
        multiple: true,
        autoUpload: false,
        // set to true to output server response to console
        debug: true,
        failUploadText: 'Arquivo ja existe.',
        extraDropzones: [$("#solteaqui")[0]],
        //extraDropzones: [$(".qq-upload-extra-drop-area")[0]],
        // events
        // you can return false to abort submit
        onSubmit: function (id, fileName) {
            
        },
        onProgress: function (id, fileName, loaded, total) {
            var percentLoaded = (loaded / total) * 100;

            //$("#progressbar").progressbar({ value: percentLoaded });
            $(".progress-bar").css('width', percentLoaded + '%');
            //$(".progress-bar").css('width', percentLoaded + '%').text(percentLoaded);
            i++;

            if (percentLoaded == 0) { alert('Concluido');}
        },
        onComplete: function (id, fileName, responseJSON) {
            //$(".progress-bar").css('width', '100%').empty(); //.text(fileName+' concluido');
            i=0;
            /*   $('a#btnEncerrarLote')
                           .bind('click', function (event) { event.preventDefault(); EncerrarLote(); });*/
            return;
        },
        onCancel: function (id, fileName) { },
        onError: function (id, fileName, xhr) {
            $('a#btnEncerrarLote')
                .unbind('click');
        },

        messages: {
            // error messages, see qq.FileUploaderBasic for content
        },
        showMessage: function (message) { alert(message); }
    });

    $('#startUpload').click(function (event) {
        event.preventDefault();
        /*
        $('a#btnGerarLote')
            .unbind('click');
        $('a#btnCancelarLote')
            .bind('click', function (event) { event.preventDefault(); CancelarLote(); });
        $('a#btnEncerrarLote')
            .bind('click', function (event) { event.preventDefault(); EncerrarLote(); });
            */
        uploader.uploadStoredFiles();

    });

}
/*var messageBox = function (params) {
    var template = '';
    var modalDialog = false;

    template += '<div title="' + (params.title || 'Mensagem') + '">';
    template += params.message;
    template += '</div>';

    modalDialog = jQuery(template);

    if (params.selector == undefined) {
        params.selector = jQuery.data(modalDialog);
    } else { modalDialog.attr('id', params.selector); }

    modalDialog.dialog({
        resizable: true,
        height: params.height || 225,
        modal: true,
        buttons: params.buttons || { 'Fechar': function () { jQuery(this).dialog('close'); } },
        close: function (event, ui) { jQuery(this).dialog('destroy'); jQuery(this).remove(); }
    });
}*/
