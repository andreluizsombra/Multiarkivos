jQuery(document).ready(function () { init(); });

var init = function () {
    bindControles();
    listar_tipos_doc();
    listar_documento_tipificar($("#hdnIdLote").val());
    $('input:text[id^="txtcampo_"]').focus();
    $("#viewer").iviewer('set_zoom', 10);
    $("#viewerPDF").pdfSlider();
}

var trocar_imagem = function (_path) {
    $("#viewer").iviewer('loadImage', _path);
    $("#viewer").iviewer('set_zoom',10);
    $("#viewer").iviewer('set_zoom',10);
}

var bindControles = function () {
    var iv1 = $("#viewer").iviewer({
        src: "/Images/sem_img.jpg",
        update_on_resize: true,
        zoom_animation: true,
        set_zoom:100,
        mousewheel: true,
        onMouseMove: function (ev, coords) { },
        onStartDrag: function (ev, coords) { }, //this image will not be dragged
        onDrag: function (ev, coords) { }
    });
    
    $("#in").click(function () { iv1.iviewer('zoom_by', 1); });
    $("#out").click(function () { iv1.iviewer('zoom_by', -1); });
    $("#fit").click(function () { iv1.iviewer('fit'); });
    $("#orig").click(function () { iv1.iviewer('set_zoom', 100); });
    $("#update").click(function () { iv1.iviewer('update_container_info'); });
    $("#btn_salvar").click(function () { tipificar_documento($("#hdnIdLote").val(), $("#hdnIdLote").val(), $("#cboTiposDoc option:selected").val()); }); //listar_documento_tipificar(0); });
    $("#btn_excluirlote").click(function () { /*locastyle.modal.open({ target: "#modal-Excluir" }); }); //listar_documento_tipificar(0);*/ });
    $("#btn_supervisor").click(function () { ajax_enviar_supervisao($("#hdnIdLote").val(), $("#hdnIdLote").val()) });
    
    $("#btn_deletarLoteModal").click(function () { ajax_exluir($("#hdnIdLote").val()) });


    var fill = false;
    $("#fill").click(function () {
        fill = !fill;
        iv1.iviewer('fill_container', fill);
        return false;
    });
}
/////////////////////////////////////////////////////////////////////
//AjaxCallExcuirDocumento
var ajax_exluir = function (_idLote) {

    var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../Tipificacao/AjaxCallEnviarLoteExcluir',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: { id_lote: parseInt(_idLote) },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }


                window.location = window.location.toString().replace(/#/gi, '').replace('/Documento/Tipificacao/Tipificar?idlote=' + _idLote, '/Documento/Tipificacao/ListarPentendes');
                    //Documento/Tipificacao/ListarPentendes

                //if (data.success == true) {
                //    //window.location = window.location.toString().replace(/#/gi, '').replace('/Digitacao/Digitar/' + _idDocumento, '/Supervisao/ListarPendentes/');
                //    listar_tipos_doc_CallBack(data);
                //}
                //else {
                //    $.unblockUI();
                //    $('div#modal-resultado-digitacao span#texto-resultado').text(data.message);
                //    ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });
                //}
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}
/////////////////////////////////////////////////////////////////////

var ajax_enviar_supervisao = function (_idLote, _idLoteItem) {
    try {
        $.ajax({
            url: '../Tipificacao/AjaxCallEnviarLoteSupervisao',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: {
                id_lote: parseInt(_idLote),
                id_item: parseInt(_idLoteItem)
            },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }
                if (data.success == true) {
                    data = data.output;
                    listar_documento_tipificar_CallBack(data);
                    $.unblockUI();
                }
                else {
                    $.unblockUI();
                    $('div#modal-resultado-tipificacao span#texto-resultado').text(data.message);

                    ////locastyle.modal.open({ target: '#modal-resultado-tipificacao' });

                    //$('div#modal-condicoes-geradas').modal('show').on('hide', function () {
                    //    window.location = window.location.toString().replace(/#/gi, '');
                    //})
                    //window.location = window.location.toString().replace(/#/gi, '');
                    //Exception.show(data.message, methodName);
                }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}

var listar_tipos_doc = function () {
    //var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../Tipificacao/AjaxCallBuscarTiposDocumento',
            dataType: 'json',
            type: 'POST',
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;
                    listar_tipos_doc_CallBack(data);
                }
                else { Exception.show(data.message, methodName); }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}

function listar_tipos_doc_CallBack(json) {
    $("#cboTiposDoc option").remove();
    $(json).each(function () {
        $("#cboTiposDoc").append("<option value='" + this.ID + "'>" + this.Descricao + "</option>");
    });
}

var listar_documento_tipificar = function (_idLote) {
    var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../Tipificacao/AjaxCallBuscarDocumentoTipificar',
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings);
            },
            data: {
                id_lote: parseInt(_idLote)
            },
            success: function (data, textstatus, xmlhttprequest) {

                //trocar_imagem("/Content/Uploads/Souza Cruz/Contratos/2014/12/23/1/U1C1S2_2014122321981.jpg");

                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;
                    listar_documento_tipificar_CallBack(data);
                    $.unblockUI();
                }
                else {
                    Exception.show(data.message, methodName);
                }
                $.unblockUI();
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}


var tipificar_documento = function (_idLote, _idLoteItem, _idDocumentoModelo) {
    var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../Tipificacao/AjaxCallTipificarDocumento',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: {
                id_lote: parseInt(_idLote),
                id_item: parseInt(_idLoteItem),
                id_documento_modelo: parseInt(_idDocumentoModelo)
            },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }
                exibirmsg('Documento tipificado com sucesso.');
                window.location.href = 'ListarPentendes';
                if (data.success == true) {
                    data = data.output;
                    listar_documento_tipificar_CallBack(data);
                    $.unblockUI();
                    
                }
                else {
                    
                    $.unblockUI();
                    $('div#modal-resultado-tipificacao span#texto-resultado').text(data.message);

                    //locastyle.modal.open({ target: '#modal-resultado-tipificacao' });
                    //$('div#modal-resultado-tipificacao').show();
                    //$('div#modal-resultado-tipificacao').modal('show').on('hide', function () {
                    //    window.location = window.location.toString().replace(/#/gi, '');
                    //})
                    //window.location = window.location.toString().replace(/#/gi, '');
                    //Exception.show(data.message, methodName);
                }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}


function listar_documento_tipificar_CallBack(json) {
    var _path = "";
    _path = json.PathImagem;
    // trocar_imagem(_path);
    //var _url = window.location.protocol + '//' + window.location.host + _path;
    var _url = window.location.protocol + '//' + window.location.host + json.CaminhoImg;

    $('span#path-arquivo').html('<a href="' + _url + '" target="_blank" style="target-new: tab;target-new: tab;">Clique Aqui</a>');
    
    //console.log(_url);
    trocar_imagem(_url);
}