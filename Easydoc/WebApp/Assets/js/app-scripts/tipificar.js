$('#pnlHeader').hide();
jQuery(document).ready(function () {
    //init();
    var AtualizarPagina = function () {
        if ($('#qtdlote').val() == 0) {
            window.location = window.location.toString().replace(/#/gi, '');
        }
        return false;
    };

    if ($('#qtdlote').val() == 0) {
        setInterval(AtualizarPagina, 10000);

    } else {
        init();
        $.unblockUI();
        return true;
    }
});

var init = function () {
    //$('#pnlHeader').slideUp('slow');
    $('#pnlHeader').hide();
    bindControles();
    listar_tipos_doc();
    listar_documento_tipificar($("#hdnIdLote").val());
   // $("#viewer img").css("top", "0px");
    $('input:text[id^="txtcampo_"]').focus();
    //$("#viewer").iviewer('set_zoom', 36);
    //$("#viewer").iviewer({ zoom: 36 });

    //$("#viewer img").removeAttr('style');

    $("#codtipodoc").focus();
    
    //$("#viewer img").removeAttr('style');

    
    $('#codtipodoc').keypress(function (e) {
        if (e.which == 13) {
            if (tipificar_documento($("#hdnIdLote").val(), $("#hdnIdLoteItem").val(), $("#codtipodoc").val()) == false) {
                e.preventDefault();
                return false;
            }
        }
    });
    
}

var trocar_imagem = function (_path) {
    
    if (_path.search(".pdf") > 0) {
        $("#viewer").hide();
        $("#imgpdf").attr("data", _path);
        $("#carousel").show();
        $("#carousel").pdfSlider({
            itemWidth: 800
            //,itemHeight: 1000
        });
        $(".pdfSlider_hideControls").hide();
    } else {
        $("#carousel").hide();
        
        //$("#viewer").iviewer({ zoom: 36 });
        //$("#viewer").iviewer('loadImage', _path);
        //$("#viewer").show();

        // TODO: 08/04/2016
        var iv1 = $("#viewer").iviewer({
            src: _path,
            zoom: "fit",
            onFinishLoad: function (ev, coords) { $("#viewer img").css("top", "0px"); }
            //zoom: "fit",
            //zoom_base: 50,
            //zoom_max: 500,
            //zoom_min: 50,
            //zoom_delta: 1.4,
            //update_on_resize: false,
            //zoom_animation: false
            //set_zoom: 100,
            //mousewheel: true,
            //onMouseMove: function (ev, coords) { },
            //onStartDrag: function (ev, coords) { }, //this image will not be dragged
            //onDrag: function (ev, coords) { },
            //onStartLoad: function (ev, coords) { $("#viewer").iviewer({ zoom: 36 }); }
        });
        // $("#viewer img").css({ position: "absolute", top: "0px", left: "0px" });
        $("#viewer img").css("top", "0px");

    }
}

var bindControles = function () {
   /* var iv1 = $("#viewer").iviewer({
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

        var fill = false;
    $("#fill").click(function () {
        fill = !fill;
        iv1.iviewer('fill_container', fill);
        return false;
    });

    $("#viewer img").removeAttr('style');
    */

    //Antes $("#btn_salvar").click(function () { tipificar_documento($("#hdnIdLote").val(), $("#hdnIdLote").val(), $("#cboTiposDoc option:selected").val()); });
    
    $("#btn_salvar").click(function () { tipificar_documento($("#hdnIdLote").val(), $("#hdnIdLoteItem").val(), $("#codtipodoc").val()); });

    //listar_documento_tipificar(0); });
    $("#btn_excluirlote").click(function () { $("#modalExcluirLote").modal('show'); /*locastyle.modal.open({ target: "#modal-Excluir" }); }); //listar_documento_tipificar(0);*/ });
    $("#btn_supervisor").click(function () { ajax_enviar_supervisao($("#hdnIdLote").val(), $("#hdnIdLote").val()) });
    
    $("#btn_deletarLoteModal").click(function () { ajax_exluir($("#hdnIdLote").val()) });

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

             window.location = window.location.toString().replace(/#/gi, '').replace('/Documento/Tipificacao/Tipificar?idlote=' + _idLote, '/Documento/Tipificacao/Tipificar');
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
        //$("#cboTiposDoc").append("<option value='" + this.ID + "'>" + this.Descricao + "</option>");
        $("#listaTipoDoc").append("<li id='li_"+this.ID+"' class='list-group-item'>" + this.ID + " - " + this.Descricao + "</li>");
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

//Quando clica no botão salvar
var tipificar_documento = function (_idLote, _idLoteItem, _idDocumentoModelo) {
    var methodName = GetMethodName(arguments.callee);
    
    if ($("#li_" + _idDocumentoModelo).length == 0) {
        exibirmsgatencao('Código não existe na lista, tente novamente...');
        return false;
    } else {

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
                    //exibirmsg('Documento tipificado com sucesso.');

                    window.location.href = 'Tipificar';
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
}


function listar_documento_tipificar_CallBack(json) {
    var _path = "";
    _path = json.PathImagem;
    
    // trocar_imagem(_path);
    //var _url = window.location.protocol + '//' + window.location.host + _path;

    //var _url = window.location.protocol + '//' + window.location.host + json.CaminhoImg;
    var _url = "";
    debugger;
    if (json.Nuvem == 1)
        _url = json.CaminhoImg
    else
        _url = window.location.protocol + '//' + window.location.host + json.CaminhoImg;

    $('span#path-arquivo').html('<a href="' + _url + '" target="_blank" style="target-new: tab;target-new: tab;">Clique Aqui</a>');
    
    //console.log(_url);
    trocar_imagem(_url);
    $("#viewer img").css("top", "0px");

    
}