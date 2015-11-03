jQuery(document).ready(function () { init(); });

var blockUIMessage = "<div class='ls-alert-info splash'>"
blockUIMessage += "<dl>";
blockUIMessage += "   <dt>Aguarde!</dt>";
blockUIMessage += "   <dt><div class='progress progress-striped active'><div class='bar' style='width: 100%;'></div></div>";
blockUIMessage += "</dl>";
blockUIMessage += "</div>";
var blockUISettings = { title: '', centerY: 15, theme: true, showOverlay: true, message: blockUIMessage };

var init = function () {
    $('#pnlHeader').slideUp('slow');
    var feeID = '';
    var datasetID = '';
    $('#pnl-busca').hide();
    $('#pnl-result').hide();
    
    bindJqGrid("SetPayInvoice", feeID, datasetID, 1);
    listar_tipos_doc();
    
    $('#add-campos').click(function () {
        AdicionaCamposFiltro($('#sel-index-99000')[0].innerHTML);
        $("button[id^='del-pnl-']").click(function () { RemoveCamposFiltro(this); });

    });
    $("button[id^='del-pnl-']").click(function () { RemoveCamposFiltro(this); });

    $('#btn-limpar').click(function () {
        
        //$('#pnl-busca').hide();
        $('#pnl-result').hide();
        $("input[id^='txtValor-']").val('');
        $("div[id^='pnl-filtro-d-']").remove();

        $("tbody").html(''); //Limpar registros da tabela de resultado

    });

    $("#btn_buscar").click(function () {
        $('#pnl-busca').hide();
        if ($("#cboTiposDoc option:selected").val() == 0) return false; ajax_buscar_campos($("#cboTiposDoc option:selected").val());
    });

    $("#btn-salvar").click(function () {
        ////locastyle.modal.open({ target: "#saveModal" });
        $('#errSalvaConsulta').hide();
    });

    $("#btn-consultas-salvas").click(function () {
        ////locastyle.modal.open({ target: "#modal-consultas-salvas" });
    });


    $("#btn-ordenar").click(function () {
        ////locastyle.modal.open({ target: "#modal-order-consulta" });
    });

    $("#btn-salvar-consulta").click(function () {
        var _nomeConsulta = $('#txtNomeConsulta').val();
        var _idDocumento = $("#cboTiposDoc option:selected").val();

        if (_nomeConsulta != '') {
            SalvarConsultaDinamica(_idDocumento, _nomeConsulta);
        } else {
            $('#errSalvaConsulta').show();
            return false;
        }
    });

    $(function () {
        $("#lst-campos, #lst-campos-sel").sortable({
            connectWith: ".connectedSortable"
        }).disableSelection();
    });

    $("#btn-pesquisar").click(function () {
        //MontaDataJSON();
        var json = MontaDataJSON();
        PesquisarDosumentos(json);
        $('#pnl-result').show();
    });

    $("#btn-edita").click(function () {
        //MontaDataJSON();
        var json = MontaDataJSON()
        PesquisarDosumentos(json);
        $('#pnl-result').show();
    });

     $("#btn_salvarModal").click(function () {                
        if (txtValor.value == "")
        {   
            alert("Informe Corretamente o motivo. O motivo não pode estar em branco!!!")
            txtValor.focus();
            return false;
        }
        else
        {
            var Valor = txtValida.value.indexOf(txtValor.value);           
            if (Valor >= 0)
            {   
                ajax_enviar_supervisao($('#IdDocumento').val(), txtValor.value);
                //return validarCampos();
                return true;
            }
            else
            {
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


    $("a[id^='btn-cons-']").click(function () {
        //MontaDataJSON();
        var _id=$(this).attr('idcon')
        SelecionaConsultaDinamica(_id);
        //PesquisarDosumentos();
        $('#pnl-result').show();
    });

}

var RemoveCamposFiltro = function ($_obj) {
    var _id = $($_obj).attr('id');
    _id = _id.replace('del-', '')
    $("#"+_id).remove();
}


function listar_tipos_doc_CallBack(json) {
    $("#cboTiposDoc option").remove();
    $("#cboTiposDoc").append("<option value='0'>Selecione um tipo de documento.</option>");
    $(json).each(function () {
        $("#cboTiposDoc").append("<option value='" + this.ID + "'>" + this.Descricao + "</option>");
    });
}
var listar_tipos_doc = function () {

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
                }
                else { Exception.show(data.message, 'ajax_buscar_campos'); }
            }
        });
    }
    catch (e) { Exception.show(e.toString(), 'ajax_buscar_campos'); }
    $.unblockUI();
}
function ajax_buscar_campos_CallBack(json) {

    var _html = '';
    var _htmlSel = '';

    $("#sel-index-99000 option").remove();
    //$("#lst-campos li").remove();
    $("#lst-campos-sel li").remove();
    $("div[id^='pnl-filtro-d-']").remove();
    
    $(json).each(function () {
        _html += '<li id="' + this.RotuloAbreviado + '" class="ui-state-default ls-cursor-move">' + this.Rotulo + '</li>';
        _htmlSel += '<option value="' + this.RotuloAbreviado + '">' + this.Rotulo + '</option>';
    });
    
    if (_html != '') {
        $('#pnl-busca').show();
        $('#lst-campos-sel').append(_html);
        $('#sel-index-99000').append(_htmlSel);
    }
}
function bindJqGrid(actionController, feeID, datasetID, step)
 {
    agreementID = $("#agreementID").val();

    mappingTemplateID = $("#mappingTemplateID").val();
    invoiceID = $("#invoiceID").val();
    var action = '../Consulta/AjaxCallGridHeadData';
    var caption = "Invoice Exception";
    $("#headerText").html(caption);
}


var PesquisarDosumentos = function (json) {
    
    //var json = MontaDataJSON();

    var jsonDataName = "";
    var jsonDataModel = "";
    var jsonWhere = "";
    var jsonExecucao = "";

    _json = eval("(" + json + ")");

    jsonDataName = _json.th;
    jsonDataModel = eval('{' + JSON.stringify(_json.tr).replace('"#', '').replace('#"', '') + '}');
    jsonWhere = _json.where;
    jsonExecucao = _json.exec;

    var strWhere = '';
    var strSelect = '';

    $(jsonDataModel).each(function (_index) {

        if (this.jsonmap != 'IdDocumento' && this.jsonmap != 'PathArquivo') {
            if (strSelect == '') {
                strSelect = this.jsonmap;
            } else {
                strSelect += ', ' + this.jsonmap;
            }
        }
    });

    $(jsonWhere).each(function (_index) {
        if (strWhere == '') {
            strWhere = this.sel + ' ' + this.op + ' ' + this.val;
        } else {
            strWhere += ' ' + this.x + ' ' + this.sel + ' ' + this.op + ' ' + this.val;
        }
    });
    MontarJQGrid(parseInt(jsonExecucao.idoc), strSelect, strWhere, jsonExecucao.proc, jsonDataName, jsonDataModel);
}




var SelecionaConsultaDinamica = function (_id_consulta_modelo) {

    var json = '';//MontaDataJSON();
    
    try {
        $.ajax({
            url: '../Consulta/AjaxCallSelecionaConsultaDinamica',
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: {
                id_consulta_modelo: parseInt(_id_consulta_modelo)
            },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;
                    //ajax_buscar_campos_CallBack(data);

                    
                    PesquisarDosumentos(data);

                }
                else { Exception.show(data.message, 'ajax_buscar_campos'); }
                ////locastyle.modal.close({ target: "#saveModal" });
            }
        });
    }
    catch (e) { Exception.show(e.toString(), 'ajax_buscar_campos'); }
    $.unblockUI();
}





//AjaxCallSalvarConsultaDinamica
var SalvarConsultaDinamica = function (_id_documento_modelo, _nome_consulta) {

    var json = MontaDataJSON();
    
    try {
        $.ajax({
            url: '../Consulta/AjaxCallSalvarConsultaDinamica',
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: {
                id_documento_modelo: parseInt(_id_documento_modelo),
                nome_consulta: _nome_consulta,
                string_json: json
            },
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;
                    ajax_buscar_campos_CallBack(data);
                }
                else { Exception.show(data.message, 'ajax_buscar_campos'); }
                ////locastyle.modal.close({ target: "#saveModal" });
                alert('Consulta salva com sucesso!');
                $('#txtNomeConsulta').val('');
            }
        });
    }
    catch (e) { Exception.show(e.toString(), 'ajax_buscar_campos'); }
    $.unblockUI();
}

var MontarJQGrid = function (id_documento, _campos,_where, _procedure, jsonDataName, jsonDataModel) {
    $("#tblGrid").jqGrid('GridUnload');
    $("#tblGrid").jqGrid({
        jsonReader: {
            id: "0",
            repeatitems: false
        },
        url: '../Consulta/AjaxCallConsultaDinamica',//action,
        datatype: 'json',
        mtype: 'POST',
        postData: {
            id_documento_modelo: parseInt(id_documento),//parseInt(jsonExecucao.idoc),
            campos: _campos,
            filtros: _where,
            proc_name: _procedure,//jsonExecucao.proc
        },
        ajaxGridOptions: {
            global: false
        },
        colNames: jsonDataName,
        colModel: eval(jsonDataModel),
        autowidth: true,
        altRows: true,
        shrinkToFit: true,
        sortable: true,
        cmTemplate: { sortable: true },
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
        pager: '#tblGridpager',
        hoverrows: true,
        viewrecords: true,
        multiselect: false,
        width: 500,
        height: 300,
        hidegrid: false,
        loadui: 'block',
        altclass: 'alt-row-class',
        caption: ''
    });
}

function JQGrid(caption)
{
    $("#tblGrid").jqGrid('GridUnload');
    var _idDocumentoModelo = parseInt($("#cboTiposDoc option:selected").val());

    $.ajax({
        url: '../Consulta/AjaxCallGridHeadData',
        dataType: 'json',
        type: 'POST',
        beforeSend: function (xhr) { $.blockUI(blockUISettings); },
        data: {
            id_documento_modelo: parseInt(_idDocumentoModelo)
        },
        
        success: function (data, textstatus, xmlhttprequest) {
            if (data == null) { return; }

            if (data.success == true) {
                result = data;
            //if (result) {
                if (!result.Error && result != "" && result != undefined) {
                    $("#TotalData").html(result.records);
                    $("#divWorkflowWrapper").show();
                    $("#dvFooter").show();

                    //var colData = "";
                    //colData = columnsData(result.output);
                    //colData = eval('{' + colData + '}');

                    var jsonDataName = "";
                    var jsonDataModel = "";
                    var jsonWhere = "";
                    var jsonExecucao = "";
                    var _json = "";
                    
                    _json = eval("(" + data.output + ")");

                    jsonDataName = _json.th;//eval(columnsDataModel(data.output)); 
                    jsonDataModel = eval('{' + JSON.stringify(_json.tr).replace('"#', '').replace('#"', '') + '}');
                    jsonWhere = _json.where;
                    jsonExecucao = _json.exec;

                    var strWhere = '';
                    var strSelect = '';

                    $(jsonDataModel).each(function (_index) {
                        
                        if (this.jsonmap != 'IdDocumento' && this.jsonmap != 'PathArquivo') {
                            if (strSelect == '') {
                                strSelect = this.jsonmap;
                            } else {
                                strSelect += ', ' + this.jsonmap;
                            }
                        }
                    });

                    $(jsonWhere).each(function (_index) {
                        if (strWhere == '') {
                            strWhere = this.sel + ' ' + this.op + ' ' + this.val;
                        } else {
                            strWhere += ' ' + this.x + ' ' + this.sel + ' ' + this.op + ' ' + this.val;
                        }
                    });

                    // JQGRID
                }
            }
            else {
                $("#divWorkflowWrapper").hide();
                $("#dvFooter").hide();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            if (xhr && thrownError) {
                alert('Status: ' + xhr.status + ' Error: ' + thrownError);
            }
        }
    });
}

function FormatterLinkOpenArquivo(cellvalue, options, rowObject) {

    //var _url = window.location.protocol + '//' + window.location.host + '/ImageStorage/' + cellvalue;
    //var _ret = '<center><table border=0 cellspacing=10 cellpadding=10 ><tr><td> ';
    //_ret +='<a href=' + _url + ' class="ls-btn-primary" target="_blank" style="target-new: tab;target-new: tab;">Visualizar</a>';
    //_ret += ' </td><td> ';
    //_ret += '<a id="btn_Editar" class="ls-btn-primary" href="/Documento/Digitacao/Digitar/' + rowObject.IdDocumento + '">Editar</a> ';
    //_ret += '</td></tr><table></center>'


    var _url = window.location.protocol + '//' + window.location.host + '/ImageStorage/' + cellvalue;
    var _ret = '';
    _ret += '<a href=' + _url + ' class="ls-btn-primary" target="_blank" style="target-new: tab;target-new: tab;"><span class="glyphicon glyphicon-picture"></span></a>&nbsp;&nbsp;';
    _ret += '<a id="btn_Editar" class="ls-btn-primary" href="/Documento/Digitacao/Digitar/' + rowObject.IdDocumento + '"><span class="glyphicon glyphicon-pencil"></span></a> ';

    return _ret;
    
}

function objedit(id, cellname, value) {

    var flag = 0;
    for (var i = 0; i < index; i++) {
        if (obj[i][0] == id && obj[i][1] == cellname) {
            obj[i] = [id, cellname, value]
            flag++;
        }
    }
    if (flag == 0) {
        obj[index] = [id, cellname, value];
        index++;
    }
}
function columnsDataName(json) {
    var _cn = '{}';
    $(json).each(function () {

        _cn = this;
        //_cn = this.colNames;
        //_html += monta_input(this.Rotulo, 'pesq_' + this.RotuloAbreviado + '_' + this.ID, this.ControleWEB, this.Rotulo, this.Descricao, this.Descricao, this.MaxLength, this.Requerido, this.ClasseCSS, this.Tabulacao);
    });

    return _cn;
}
function columnsDataModel(json) {
    var _cm = '{}';
    $(json).each(function () {
        _cm = this;
        //_html += monta_input(this.Rotulo, 'pesq_' + this.RotuloAbreviado + '_' + this.ID, this.ControleWEB, this.Rotulo, this.Descricao, this.Descricao, this.MaxLength, this.Requerido, this.ClasseCSS, this.Tabulacao);
    });

    return _cm;
}

/*

                sJson = @"{'th': ['ID', 'Matricula', 'Caixa', 'Período','Lote','Arquivo'],
                            'tr': [
                                            { name: 'IdDocumento',  index: 'IdDocumento',   jsonmap: 'IdDocumento', align: 'left',      width: 40, key: true},
                                            { name: 'Matricula',    index: 'Matricula',     jsonmap: 'Matricula',   align: 'center',    width: 100 },
                                            { name: 'Caixa',        index: 'Caixa',         jsonmap: 'Caixa',       align: 'center',    width: 100 },
                                            { name: 'Periodo',      index: 'Periodo',       jsonmap: 'Periodo',     align: 'center',    width: 100 },
                                            { name: 'Lote',         index: 'Lote',          jsonmap: 'Lote',        align: 'center',    width: 100 },
                                            { name: 'PathArquivo',  index: 'PathArquivo',   jsonmap: 'PathArquivo', align: 'left',      width: 30, sortable: false, formatter: '#FormatterLinkOpenArquivo#'}
                                        ],
                            'where': 
                            [
                                { sel: 'matricula', op: '=', val: '7880123456' },
                                { sel: 'matricula', op: '=', val: '7880123456', x:'and'},
                            ],
                            'exec':{proc:'proc_consulta_padrao', idoc: 1, par:''}
                        }";
*/

var MontaDataJSON = function () {

    var _str = '{'

    var _strTH = '"th": ["ID", ';
    var _strTR = '"tr": [{ name: "IdDocumento",  index: "IdDocumento",   jsonmap: "IdDocumento", align: "center",  width: 20, key: true},';

    $('#lst-campos-sel li').each(function () {
        _strTH += '"' + this.innerText + '", ';
        _strTR += '{ name: "' + this.innerText + '",    index: "' + this.id + '",     jsonmap: "' + this.id + '",   align: "center",  width: 20},';
    });
    
     _strTR = _strTR + '{ name: "PathArquivo",  index: "PathArquivo",   jsonmap: "PathArquivo", align: "center",      width: 25, sortable: false, formatter: "#FormatterLinkOpenArquivo#"}], ';
     _strTH = _strTH + '"Arquivo"],';

    var _strWHERE = '"where": [';

    $('div[id^="pnl-filtro"]').each(function () {
        var _id = '';
        _id = this.id.replace('pnl-filtro-f-', '');
        _id = _id.replace('pnl-filtro-d-', '');

        //{ sel: 'matricula', op: '=', val: '7880123456' },
        _strWHERE += '{ sel: "' + $('#sel-index-' + _id + ' option:selected').val() + '", op: "' + $('#sel-operador-' + _id + ' option:selected').val() + '", val: "\'' + $('#txtValor-' + _id).val() + '\'", x: "' + $('#sel-condicao-' + _id + ' option:selected').val() + '" },'
    });
    _strWHERE = _strWHERE + '],';
    _strEXEC = '"exec":{proc:"proc_consulta_dinamica_padrao", idoc: "' + $("#cboTiposDoc option:selected").val() + '", par:""}';

    _str = _str + _strTH + _strTR + _strWHERE + _strEXEC + '}';

    //_str = _str + _strTH + _strTR   + '}';

    return _str;
}

function columnsData(Data) {

    var formater = "";

    var str = "[";
    for (var i = 0; i < Data.length; i++) {
        if (Data[i] == "Id")
            str = str + "{name:'" + Data[i] + "', index:'" + Data[i] + "', hidden: true,}";
        else
            str = str + "{name:'" + Data[i] + "', index:'" + Data[i] + "', editable: true}";
        if (i != Data.length - 1) {
            str = str + ",";
        }
    }
    str = str + "]";
    return str;
}
//------end grid part----------
function NewGuid()
{
    var sGuid="";
    for (var i=0; i<32; i++)
    {
        sGuid+=Math.floor(Math.random()*0xF).toString(0xF);
    }
    return sGuid;
}

var AdicionaCamposFiltro = function (data) {

    var _id = NewGuid();
    _id = _id.substr(15,5);

    var _html = '';
    _html += ' <div id="pnl-filtro-d-' + _id + '" class="row col-md-12 ls-xs-space">';
    _html += '      <div class="col-md-2">';
    _html += '          <div class="ls-custom-select col-md-10">';
    _html += '              <select id="sel-condicao-' + _id + '" class="form-control">';
    _html += '                  <option value="AND">E</option>';
    _html += '                  <option value="OR">OU</option>';
    _html += '              </select>';
    _html += '          </div>';
    _html += '      </div>';

    _html += '      <div class="col-md-3">';
    _html += '              <select id="sel-index-' + _id + '" class="form-control">';
    _html += data;
    _html += '              </select>';

/*
    _html += '              <select id="sel-indexador" class="ls-select">';
    _html += '                  <option value="">Matricula</option>';
    _html += '                  <option value="0"> Lote </option>';
    _html += '                  <option value="1"> Caixa </option>';
    _html += '              </select>';

*/
    _html += '      </div>';
    _html += '      <div class="col-md-2">';
    //_html += '          <div class="ls-custom-select">';
    _html += '                  <select id="sel-operador-' + _id + '" class="form-control" >';
    _html += '                      <option value="="> Igual </option>';
    //_html += '                  <option value="!="> Diferente </option>';
    _html += '                      <option value=">"> Maior </option>';
    //_html += '                  <option value=">="> Maior Igual </option>';
    _html += '                      <option value="<"> Menor </option>';
    //_html += '                  <option value="<="> Menor Igual </option>';
    _html += '                  </select>';
    _html += '      </div>';
    _html += '      <div class="col-md-3">';
    _html += '          <input id="txtValor-' + _id + '" type="text" class="form-control" name="nome" placeholder="Valor" required>';
    _html += '      </div>';
    /*
    _html += '      <div class="col-md-1">';
    _html += '          <input type="checkbox" class="ls-field-lg" name="ordernador">';
    _html += '      </div>';
    */
    _html += '      <div class="col-md-1"><button id="del-pnl-filtro-d-' + _id + '" class="btn btn-danger">-</button></div>';
    _html += ' </div><p></p>';

    $('#pnl-busca-campos').append(_html);

}