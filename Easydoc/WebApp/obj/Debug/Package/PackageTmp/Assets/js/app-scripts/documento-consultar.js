jQuery(document).ready(function () { init(); });

var init = function () {
    $('#pnl-busca').hide();
    bindControles();
    listar_tipos_doc();
}


var bindControles = function () {
    $("#btn_buscar").click(function () {
        $('#pnl-busca').hide();
        if ($("#cboTiposDoc option:selected").val() == 0) return false; ajax_buscar_campos($("#cboTiposDoc option:selected").val());
    });

    ////////////////////////////////////////////////////////////////////
    $("#btn_Editar").click(function () {
        alert('oi');
        return true;
    });
    ////////////////////////////////////////////////////////////////////



    $("#btn-limpar").click(function () {
        $('#pnl-result').hide(); $("input:text").val('')
    });

    $("#btn-pesquisar").click(function () {
        if ($("input[name='Matricula']").val() == '') {

            $.unblockUI();
            $('div#modal-resultado-consulta span#texto-resultado').text("Favor informar o número da matrícula!");
            ////locastyle.modal.open({ target: '#modal-resultado-consulta' });
            $("input[name='Matricula']").focus();
            return false;
        }
        
        $_txtPesquisa = $('input:text[id^="pesq_"]');
        
        //$_txtPesquisa = $('#pesq_');
        var $_SQL = '';
        $_txtPesquisa.each(function (_index) {
            var _valor = this.value;
            var _arrData = '';

            var _dataFormatada = '';
            if (this.className == 'datepicker ls-mask-date') {
                _arrData = _valor.split('/');
                _dataFormatada = _arrData[2];
                _dataFormatada += _arrData[1];
                _dataFormatada += _arrData[0];
                _valor = _dataFormatada;
            }
            
            if (this.value != '') {
                if ($_SQL == '') {
                    $_SQL += this.name + " = '" + _valor + "'";
                } else {
                    $_SQL += " AND " + this.name + " = '" + _valor + "'";
                }
            }

        });
        ajax_pesquisar_documentos($("#cboTiposDoc option:selected").val(), $_SQL);
    });
}

var ajax_pesquisar_documentos = function (_idDocumentoModelo, _where) {

    try {
        $.ajax({
            url: '../Consulta/AjaxCallPesquisarDocumentos',
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: {
                id_documento_modelo: parseInt(_idDocumentoModelo),
                filtros: _where
            },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;

                    var _html = '';

                    if (data != '') {
                        _html += '<table class="ls-table ls-bg-header ls-table-striped">';
                        _html += '<thead>';
                        _html += '<tr>';
                        _html += '<th style="width:150px;">ID</th>';
                        

                        _html += '<th class="hidden-xs">Matricula</th>';
                        _html += '<th>Caixa de Guarda</th>';
                        _html += '<th>Período</th>';
                        _html += '<th>Lote</th>';
                        _html += '<th class="hidden-xs">Arquivo</th>';
                        _html += '</tr>';
                        _html += '</thead>';

                        $(data).each(function () {
                            _html += '<tr>';
                            _html += '<td class="hidden-xs" style="width:150px;">' + this.IdDocumento + '</td>';
                            _html += '<td class="hidden-xs">' + this.Matricula + '</td>';
                            _html += '<td class="hidden-xs">' + this.Caixa + '</td>';
                            _html += '<td class="hidden-xs">' + this.Período + ' </td>';
                            _html += '<td class="hidden-xs">' + this.Lote + ' </td>';

                            var _url = window.location.protocol + '//' + window.location.host + '/StoragePrivate/' + this.PathArquivo;
                            _html += '<td><a href="' + _url + '" title="Arquivo do docuemnto" target="_blank" style="target-new: tab;target-new: tab;">Vizualizar</a></td>';
                            _html += '</tr>';

                        });
                        _html += '</tbody>';
                        _html += '</table>';

                    } else {
                        _html = "<h2>SEM RESULTADO PARA ESTA PESQUISA</2>";
                    }
                    
                    $('#pnl-result').show();
                    $('#pnl-result').html(_html);
                    //ajax_buscar_campos_CallBack(data);
                }
                else { Exception.show(data.message, "ajax_pesquisar_documentos"); }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    }
    catch (e) { Exception.show(e.toString(), "ajax_pesquisar_documentos"); }
    $.unblockUI();

}

var ajax_buscar_campos = function (_idDocumentoModelo) {
    try {
        $.ajax({
            url: '../Consulta/AjaxCallBuscarCamposModelo',
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: {
                id_documento_modelo: parseInt(_idDocumentoModelo)
            },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;
                    ajax_buscar_campos_CallBack(data);
                    ConfigurarCampoData();
                    $(function () {
                        $('input:text[id^="pesq_"]').on('blur', function () { checkForEnter });
                    });
                }
                else { Exception.show(data.message, 'ajax_buscar_campos'); }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), 'ajax_buscar_campos'); }
    $.unblockUI();
}


var ConfigurarCampoData = function () {
    $('.date-mes-ano').mask('M#/2Z##', {
        'translation': {
            M: { pattern: /[0-1]/ },
            V: { pattern: /'20'[0-99]/ },
            Z: { pattern: /0/ }
        }
    });
    ////locastyle.datepicker.newDatepicker('.datepicker');
    $('.datepicker').attr('readonly', true);

}

var monta_input = function (_label, _id, _type, _name, _arial_label, _placeholder, _maxlength, _required, classe_css, _tabindex) {

    var _html = '';

    _html += '<label class="ls-label col-md-3">';
    _html += '    <b class="ls-label-text">' + _label + '</b>';
    _html += '       <input type="text" id="' + _id + '" class="' + classe_css + '" name="' + _name + '" aria-label="' + _arial_label + '" accesskey="s" placeholder="' + _placeholder + '" required="' + _required + '" maxlength="' + _maxlength + '" tabindex="' + _tabindex + '">';
    _html += '</label>';
    return _html;
}
function ajax_buscar_campos_CallBack(json) {

    
    var _html = '';
    _html += '<form action="" class="ls-form ls-form-horizontal" data-ls-module="form">'
    //_html +='<label class="ls-label col-md-3 col-xs-12">'
    $(json).each(function () {
        _html += monta_input(this.Rotulo, 'pesq_' + this.RotuloAbreviado + '_' + this.ID, this.ControleWEB, this.Rotulo, this.Descricao, this.Descricao, this.MaxLength, this.Requerido, this.ClasseCSS, this.Tabulacao);
    });
    //_html += '</label>';
    _html += '</form>'

    if (_html !='') {
        $('#pnl-busca').show();
        $('#CamposBusca').html(_html);
    }
    $('[tabindex=1]').focus();
}


function listar_tipos_doc_CallBack(json) {
    $("#cboTiposDoc option").remove();
    $("#cboTiposDoc").append("<option value='0'>Selecione um tipo de documento.</option>");
    $(json).each(function () {
        $("#cboTiposDoc").append("<option value='" + this.ID + "'>" + this.Descricao + "</option>");
    });
}

var listar_tipos_doc = function () {
    //var methodName = GetMethodName(arguments.callee);
    try {
        $.ajax({
            url: '../Consulta/AjaxCallBuscarTiposDocumento',
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;
                    listar_tipos_doc_CallBack(data);
                }
                else { Exception.show(data.message, 'listar_tipos_doc'); }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), 'listar_tipos_doc'); }
    $.unblockUI();
}

var ConverteDataYYYYMMDD = function (data) {

    if (data == '') {return "-"}
    var _ret = '';

    _ret = data.substr(6, 2) + '/' + data.substr(4, 2) + '/' + data.substr(0, 4);

    return _ret;
}

function checkForEnter(e) {
    if (event.keyCode == 13 || event.keyCode == 9) {
        var tabindex = 1 + +$(this).attr('tabindex')

        if ($('[tabindex=' + tabindex + ']', this.form).length == 0) {
            $('#btn-pesquisar').focus();
        } else {
            $('[tabindex=' + tabindex + ']', this.form).focus();
        }

        return false;
    }
}
