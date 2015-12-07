
jQuery(document).ready(function () {
    
    $('#pnlHeader').slideUp('slow');

    $('input:text[id^="txtcampo_"]').keydown(checkForEnter);

    var AtualizarPagina = function () {
        if ($('#IdDocumento').val() == 0) {
            window.location = window.location.toString().replace(/#/gi, '');
        }
        return false;
    };

    if ($('#IdDocumento').val() == 0) {
        setInterval(AtualizarPagina, 10000);
        
    } else {
        init();
        $.unblockUI();
        return true;
    }
    $("#orig").trigger("click");

    //$('input[required=true]').each(function () {
     //   alert(this.id);
    //})
});

function BoxPosicaoInicial() {
    //console.log('aqui');
    $('#boxcampos').css('margin-top', '0px');
    $('html, body').animate({ scrollTop: 0 });
 
}

function MoverCampos(campo) {
    //$('input[rotulo=Placa]').focusin(function () {
    var cmp = $("#" + campo.id);
    var tam = cmp.attr('movecampo');
    console.log(tam);
    //$('html, body').animate({ scrollTop: tam }, 'slow');
    $('#boxcampos').css('margin-top', tam + 'px');
    if(tam>0){
        $('html, body').animate({ scrollTop: tam }, 'slow');
    } else {
        $('html, body').animate({ scrollTop: 0}, 'slow');
    }

    /*cmp.focusin(function () {
        //var tam = $('input[rotulo=Placa]').attr('movecampo');
        var tam = cmp.attr('movecampo');
        console.log(tam);
        
        $('#boxcampos').css('margin-top', tam+'px');
        // }
    });
    */
}

function BoxCampoPlaca(campo) {
    var _campo = $('#' + campo.id);
    if (_campo.val() != '') {
        console.log(_campo.val());
        $('html, body').animate({ scrollTop: 400 }, 'slow');
        $('#boxcampos').css('margin-top', '500px');
    }
}

function validarCampos() {
    var $_txtCampo = $('input:text[required]');
    var $_ret = true;
    $_txtCampo.each(function (_index) {
        if (this.value == '' && this.name != 'txtValor') {
            //textbox Escollha
            exibirmsgatencao("O campo [" + this.name + "] é obrigatório.");
            ////if (this.name != 'sup') {
            //// $('div#modal-resultado-digitacao span#texto-resultado').text("O campo [" + this.name + "] é obrigatório.");
            ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });
            $_ret = false;
            ////}
        }
    });
    if ($_ret) {
        digitar_documento();
    }
}
function validarCamposAprovar() {
    var $_txtCampo = $('input:text[required]');
    var $_ret = true;
    $_txtCampo.each(function (_index) {
        if (this.value == '' && this.name != 'txtValor') {
            exibirmsgatencao("O campo [" + this.name + "] é obrigatório.");
            $_ret = false;
        }
        // TODO: AndreSombra 03/11/2015
        //if (this.value == '') {
        //    if (this.name != 'sup') {
        //        if (this.name != 'some-id') {
        //            $('div#modal-resultado-digitacao span#texto-resultado').text("O campo [" + this.name + "] é obrigatório.");
        //            ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });
        //            $_ret = false;
                    
        //        }
        //    }
        //}
    });
    if ($_ret) {
        
        digitar_documento();
        //var surl = window.location.toString().replace(/#/gi, '').replace('/Documento/Digitacao/Digitar/' + $('#IdDocumento').toString, '/Home/Inicio/');
        //var surl = 'http://' + window.location.host + '/Home/Inicio';
        //window.location = surl;
    }
}

var init = function () {
    //var _path = $("#arq").val();
    //var _url = window.location.protocol + '//' + window.location.host + '/ImageStorage/' + _path;
    //trocar_imagem(_url);
    bindControles();
    $("#viewer").show();
    $("#viewer").iviewer('set_zoom', 10);
    $('input:text[id^="txtcampo_"]').focus();
    //debugger;
    var _path = $("#arq").val();
    var _url = window.location.protocol + '//' + window.location.host + '/ImageStorage/' + _path;
    //var _url = "http://
    //ImageStorage/Souza_Cruz/RH/2015/2/25/U1C1S1_201522516438.JPG";//window.location.protocol + '//' + window.location.host + '/ImageStorage/' + _path;

    if (_path.search(".pdf") > 0) {
        $("#viewer").hide();
        $("#imgpdf").attr("data", _url);
        //$("#viewerPDF").html('<a class="media" href="' + _url + '"></a>');
        //$('#viewerPDF').show();
        //CarregarImagemPdf(_url);
        //$('a.media').media({ width: 990, height: 600 });
        //$('#pnl-imagem').attr('display','none');
    } else {
        $('#pnl-imagem').show();
        //CarregarImagem(_url);
        //$("#viewer").iviewer('loadImage', _url);

       //$("#viewer").html('<img id="doc_imagem"  class="top_aligned_image" src="' + _url + '"  >');
       // $("#viewer").iviewer('set_zoom', 50);
       //$("#viewer").iviewer('set_zoom', 50);
       // $('.guillotine-canvas').css('top', '0px');
    }

    $('span#path-arquivo').html('<a href="' + _url + '" class="ls-ico-export" target="_blank" style="target-new: tab;target-new: tab;"></a>');
    //$('input:text[id^="txtcampo_"]').focus();
    $('[tabindex=1]').focus();
    $("#orig").trigger("click");

    //$('input[rotulo=Placa]').mask('AAA-9999');
    //$('input[rotulo=Placa]').css('placeholder', 'AAA-9999');

    // TODO: AndreSombra 06/11/2015 =========================================
    $('input').each(function () {
        var campo = $("#" + this.id);
        var _mascara = campo.attr('mascara'); //Retorna o valor do atributo mascara
        //console.log(campo);
        //console.log(this.id);
        //console.log(campo.mascara);
        console.log(_mascara);
        if ( _mascara!= '') {
            campo.mask(_mascara);
        } else {

            //Verificar campo caso seja retornado valor 0 do atributo 'maiuscula', entao executa função Minusculo AndreSombra 07/12/2015
            if (campo.attr('maiuscula') == 0)
                Minusculo(campo);
            else
                Maiusculo(campo);
        }
        

    });

    // TODO: AndreSombra 24/11/2015 =========================================
    $('input').each(function () {
        var campo = $("#" + this.id);
        campo.attr('onfocus', 'MoverCampos(this)');

        
    });


   // BoxPosicaoInicial(); //Colocar o box na posicao incial.
   // console.log('aqui 2');

}

//Colocar texto em Maiusculo
//Exemplo: Maiusculo("#nome");
function Maiusculo(campo) {
    $(campo).keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });
}
//Colocar texto em Minusculo
function Minusculo(campo) {
    $(campo).keyup(function () {
        $(this).val($(this).val().toLowerCase());
    });
}

var trocar_imagem = function (_path) {
    $("#viewer").iviewer('loadImage', _path);
    $("#viewer").iviewer('set_zoom', 10);
    $("#viewer").iviewer('set_zoom', 10);
}

function checkForEnter(e) {
    if (event.keyCode == 13 || event.keyCode == 9) {
        var tabindex = 1 + +$(this).attr('tabindex')

        if ($('[tabindex=' + tabindex + ']', this.form).length == 0) {
            $('#btn_salvar').focus();
        } else {
            $('[tabindex=' + tabindex + ']', this.form).focus();
        }

        return false;
    }
}
$(function () {
    //$('#elementoPai').delegate('#elemento', 'blur', function(){ ...});
    $('input:text[id^="txtcampo_"]').on('blur', function () { checkForEnter });
})

var CarregarImagem = function (_url) {
    //AndreSombra
    $("#viewer").html('<img id="doc_imagem"  class="top_aligned_image" src="' + _url + '"  >');
    //var picture = $('#doc_imagem');
    
    //picture.guillotine({
    //    init: { w: 0, x: 0, y: 0, angle: 0 }
    //});
    ////$('.guillotine-canvas').css('top', '0px');
    //picture.on('load', function () {
    //    // Initialize plugin (with custom event)
    //    picture.guillotine({
    //        eventOnChange: 'guillotinechange', width: 0,
    //        height: 0
    //        //, nit: { x: 10, y: 60, angle: 90 }
    //    }
    //    );
    //    // Bind button actions
    //    $('#rotate_right').click(function () { picture.guillotine('rotateRight'); });
    //    $("#fit").on("click", function () { picture.guillotine('fit'); });
    //    $('#zoom_in').click(function () { picture.guillotine('zoomIn'); });
    //    $('#zoom_out').click(function () { picture.guillotine('zoomOut'); });
    //    $("#fit").trigger("click");
    //    // Display inital data
    //    var data = picture.guillotine('getData');
    //    for (var key in data) { $('#' + key).html(data[key]); }
    //    // Update data on change
    //    /*picture.on('guillotinechange', function (ev, data, action) {

    //        data.scale = parseFloat(data.scale.toFixed(4));
    //        for (var k in data) { $('#' + k).html(data[k]); }
    //    });*/
    //    $("#fit").trigger("click");
    //});

   
}

var bindControles = function () {

    $('img').css('top', '0px');

    var _path = $("#arq").val();
    var _url = window.location.protocol + '//' + window.location.host + '/ImageStorage/' + _path;

    var iv1 = $("#viewer").iviewer({
        src: _url,
        update_on_resize: true,
        zoom_animation: true,
        set_zoom: 100,
        mousewheel: false,
        onMouseMove: function (ev, coords) { },
        onStartDrag: function (ev, coords) { }, //this image will not be dragged
        onDrag: function (ev, coords) { }
    });

    $("#viewer").iviewer('set_zoom', 10);
    $("#viewer").iviewer('set_zoom', 10);

    $("#in").click(function () { iv1.iviewer('zoom_by', 1); });
    $("#out").click(function () { iv1.iviewer('zoom_by', -1); });
    $("#fit").click(function () { iv1.iviewer('fit'); });
    $("#orig").click(function () { iv1.iviewer('set_zoom', 100); });
    $("#fit").trigger("click");
    $("#btn_salvar").click(function () {
        // alert('teste aqui');
        ////locastyle.modal.open({ target: "#modal-duplicidade" });
        return validarCampos();
    });

    $("#btn_salvarDup").click(function () {


        var status1 = $('#txtstatus').attr('value');
        if (status1 == 1020) {

            validarCamposAprovar();
            ajax_Aprovar($('#IdDocumento').val());

        }
        else {
            return validarCampos()
        }


    });

    //walmir
    $("#btn_Aprovar").click(function () {

        //validarCamposAprovar();
        //ajax_Aprovar($('#IdDocumento').val());

        var test = $("#some-id").attr("value");
        test = test.replace(";", "");
        if (test == '') { test = null }
        if (test != null) {
            ////locastyle.modal.open({ target: "#modal-duplicidade" });
        }
        else {
            validarCamposAprovar();
            ajax_Aprovar($('#IdDocumento').val());
        }



    });
    $("#btn_salvarModal").click(function () {
        if (txtValor.value == "") {
            alert("Informe Corretamente o motivo. O motivo não pode estar em branco!!!")
            txtValor.focus();
            return false;
        }
        else {
            
            var Valor = txtValida.value.indexOf(txtValor.value)*-1;
            
            if (Valor >= 0) {
                ajax_enviar_supervisao($('#IdDocumento').val(), txtValor.value);
                //return validarCampos();
                return true;
            }
            else {
                alert("Informe Corretamente o motivo. Opção invalida!!!")
                txtValor.focus();
                return false;
            }
        }

        //var r = confirm("Confirma a exclusão do documento " + ViewData["N"] + "?");
        ////var r = confirm("aaaa?");
        //if (r == true) {
        //    return false;
        //   // ajax_exluir($('#IdDocumento').val());
        //} else {
        //    return false;
        //}

    });
    $("#btn_salvarModalExcluir").click(function () {
        //this.btnAprovar.disabled = false;
        if (txtValor.value == "") {
            alert("Informe Corretamente o motivo. O motivo não pode estar em branco!!!")
            txtValor.focus();
            return false;
        }
        else {
            var Valor = txtValida.value.indexOf(txtValor.value);
            if (Valor >= 0) {
                ajax_exluir($('#IdDocumento').val(), txtValor.value);
                //ajax_enviar_supervisao($('#IdDocumento').val(), txtValor.value);               
            }
            else {
                alert("Informe Corretamente o motivo. Opção invalida!!!")
                txtValor.focus();

            }
        }
    });
    $("#btn_salvarModalDup").click(function () {

        var myRadio1 = $('input[name=rdndup]');
        var value1 = myRadio1.filter(':checked').val();

        if (value1 == null) {
            alert("Informe Corretamente a data!!!")
            return false;
        }
        else {
            ajax_ValidaDuplicidade($('#IdDocumento').val(), value1);
        }

    });
    $("#btn_enviaSupervisaoLiderado").click(function () {
        ajax_ValidaDuplicidade($('#IdDocumento').val(), 8);
        //return true;
    });
    $("#btn_supervisor").click(function () {

        ////locastyle.modal.open({ target: "#modal-supervisao" });
        $('#modal-supervisao').modal();
        txtValor.focus();

    });
    $("#btn_excluir").click(function () {
        ////locastyle.modal.open({ target: "#modal-supervisao" });
        // TODO: AndreSombra 03/11/2015
        $('#modal-supervisao').modal();
        txtValor.focus();
    });
    $("#btn_voltapesquisa").click(function () {
        history.go(-1);
    });

}

var digitar_documento = function () {
    var $_retorno = gerar_json_documento();
    ajax_digitar_documento($_retorno);
}

var ajax_digitar_documento = function ($_campos_json) {

    //var methodName = GetMethodName(arguments.callee);
    var $_idDocumentoModelo = $('#IdDocumentoModelo').val();
    try {
        var jqxhr = $.ajax({
            url: '../../Digitacao/AjaxCallDigitarDocumento',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: { id_documento_modelo: $_idDocumentoModelo, documento_digitado: $_campos_json },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) {
                    alert('Nenhum documento.')
                    return;
                }
                if (data.success == true) {

                    //var status = $('input[name=txtstatus]').value;
                    var status1 = $('#txtstatus').attr('value');
                    if (status1 == 1020) {
                        //exibirmsg('aqui status 1020');
                        var surl = "http://" + window.location.host + '/Documento/Supervisao/ListarPendentes';
                        window.location = surl;
                        //window.location = window.location.toString().replace(/#/gi, '').replace('/Digitacao/Digitar/' + _idDocumento, '/Supervisao/ListarPendentes/');
                    }
                    else {
                        
                        window.location = window.location.toString().replace(/#/gi, '');

                       // exibirmsg('aqui teste 1');
                    }
                }
                else {
                    exibirmsgatencao('Erro, ' + data.message);
                    $('div#modal-resultado-digitacao span#texto-resultado').text(data.message);
                    ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });

                    //this.btnAprovar.disabled = false;

                    $('#btnClose').on('click', function () {
                        $('[tabindex=1]').focus();
                    });

                }
            },
            complete: function () {
                $.unblockUI();
            }
        });
        jqxhr.always(function () {
            //$('[tabindex=1]').focus();
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }

}

//AjaxCallExcuirDocumento
var ajax_exluir = function (_idDocumento, _idMotivo) {

    var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../../Digitacao/AjaxCallExcuirDocumento',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: { id_documento: parseInt(_idDocumento), id_motivo: parseInt(_idMotivo) },
            //data: { id_documento: _idDocumento },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }
                if (data.success == true) {
                    $.unblockUI();
                    window.location = window.location.toString().replace(/#/gi, '').replace('/Digitacao/Digitar/' + _idDocumento, '/Supervisao/ListarPendentes/');
                }
                else {
                    $.unblockUI();
                    $('div#modal-resultado-digitacao span#texto-resultado').text(data.message);
                    ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });
                }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}

//AjaxCallValidaDuplicidade
var ajax_ValidaDuplicidade = function (_idDocumento, _id) {

    var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../../Digitacao/AjaxCallValidaDuplicidade',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: { id_documento: parseInt(_idDocumento), id_: parseInt(_id) },
            //data: { id_documento: _idDocumento },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }
                if (data.success == true) {
                    $.unblockUI();
                    if (_id == 8 || _id >= 20) {
                        //window.location = window.location.toString().replace(/#/gi, '').replace('/Digitacao/Digitar/' + _idDocumento, '/Consulta/MontarConsulta/');
                        //window.location = window.location.toString().replace(/#/gi, '').replace('/Documento/Digitacao/Digitar/' + _idDocumento, '/Home/Inicio/');
                        window.location = window.location.toString().replace(/#/gi, '').replace('/Documento/Digitacao/Digitar/' + _idDocumento, '/Documento/Menu/');
                    }
                    else {
                        window.location = window.location.toString().replace(/#/gi, '').replace('/Digitacao/Digitar/' + _idDocumento, '/Supervisao/ListarPendentes/');
                    }
                }
                else {
                    $.unblockUI();
                    $('div#modal-resultado-digitacao span#texto-resultado').text(data.message);
                    ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });
                }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}

var ajax_enviar_supervisao = function (_idDocumento, _idMotivo) {

    var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../../Digitacao/AjaxCallEnviarDocumentoSupervisao',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: { id_documento: parseInt(_idDocumento), id_motivo: parseInt(_idMotivo) },
            //data: { id_documento: parseInt(_idDocumento)},
            success: function (data, textstatus, xmlhttprequest) {
                //alert("SAi 0")
                if (data == null) {
                    //alert("SAi 1");
                    return;
                }
                //alert("SAi 2");
                if (data.success == true) {
                    //alert("SAi 3");
                    $.unblockUI();
                    window.location = window.location.toString().replace(/#/gi, '');
                }
                else {
                    //alert("SAi 4");
                    $.unblockUI();
                    $('div#modal-resultado-digitacao span#texto-resultado').text(data.message);
                    ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });
                }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}

var ajax_Aprovar = function (_idDocumento) {

    var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../../Digitacao/ajax_Aprovar',
            cache: false,
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: { id_documento: parseInt(_idDocumento) },
            //data: { id_documento: parseInt(_idDocumento)},
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) {
                    return;
                }
                if (data.success == true) {
                    $.unblockUI();
                    //window.location = window.location.toString().replace(/#/gi, '');
                    //window.location = window.location.toString().replace(/#/gi, '').replace('/Documento/Digitacao/Digitar/' + _idDocumento, '/Home/Inicio/');
                    window.location = window.location.toString().replace(/#/gi, '').replace('/Documento/Digitacao/Digitar/' + _idDocumento, '/Documento/Supervisao/ListarPendentes/');
                    //var surl = window.location.host + '/Documento/Supervisao/ListarPendentes';
                }
                else {

                    var status = $('input[name=txtstatus]').value;
                    if (status != 1020) {

                        $.unblockUI();
                        $('div#modal-resultado-digitacao span#texto-resultado').text(data.message);
                        ////locastyle.modal.open({ target: '#modal-resultado-digitacao' });

                    }
                }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), methodName); }
}


var gerar_json_documento = function () {
    var $_retorno = '{';
    var $_camposDigitados = $('input:text[id^="txtcampo_"]');
    var $_idDocumento = $('#IdDocumento').val();

    $_retorno += '"Documento": {"IdDocumento":' + $_idDocumento + ', "Campos":[';
    $_camposDigitados.each(function (_index) {

        var $_idDocCampo = $(this).attr('id');

        var $_idCampoModelo = $(this).attr('campo');
        $_idDocCampo = $_idDocCampo.replace('txtcampo_', '');

        var _mascaraSaida = '';
        _mascaraSaida = $(this).attr('mascaraSaida');        //alert($(this).attr('mascaraSaida')); //*************************
        if (_mascaraSaida != '') {
            $(this).mask(_mascaraSaida);
        }

        var $_valorCampo = $(this).val();
                                   // alert('Valor Campo= ' + $_valorCampo.text); //**************************
        $_retorno += JSON.stringify({
            ID: $_idCampoModelo, IndexDoc: $_idDocumento, IndexUI: $_idDocCampo, Valor: $_valorCampo
        });
        $_retorno += ',';
    });

    $_retorno = $_retorno.substr(0, $_retorno.length - 1);
    $_retorno += ']}}';
    return $_retorno;
}
