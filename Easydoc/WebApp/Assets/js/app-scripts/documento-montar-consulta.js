jQuery(document).ready(function () {

    init();
});

var blockUIMessage = "<div class='ls-alert-info splash'>"
blockUIMessage += "<dl>";
blockUIMessage += "   <dt>Aguarde!</dt>";
blockUIMessage += "   <dt><div class='progress progress-striped active'><div class='bar' style='width: 100%;'></div></div>";
blockUIMessage += "</dl>";
blockUIMessage += "</div>";
var blockUISettings = { title: '', centerY: 15, theme: true, showOverlay: true, message: blockUIMessage };

var init = function () {

    $('#pnlHeader').slideUp('slow');

    //$("#tblDetalhe").hide();
    $("#pnl_resultado_detalhe").hide();

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
        //ColunaAuto();
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
        ColunaAuto();
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

    //TODO: AndreSombra 10/11/2015
    $("#sel-index-99000").change(function () {
        var _mascara = $("#sel-index-99000 option:selected" ).attr("mascara");
        //alert(_mascara);
        //console.log(_mascara);
        //TODO: AndreSombra 11/11/2015
        if (_mascara != '') {
            $("#txtValor-99000").mask(_mascara);
            $("#txtValor-99000").focus();
        }
        else {
            $("#txtValor-99000").unmask();
            $("#txtValor-99000").focus();
        }
        FiltraOperador(2);
    });

    $("#tblGrid").change(function () {
        ColunaAuto();
    });

    $("#btn_fechar_detalhe").click(function () {
        $("#pnl_parametros").show();
        $("#pnl_resultado").show();
        $("#pnl_resultado_detalhe").hide();
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
        //_htmlSel += '<option value="' + this.RotuloAbreviado + '">' + this.Rotulo + '</option>';
        _htmlSel += '<option mascara="' + this.MascaraEntrada + '" mascarasaida="' + this.MascaraSaida + '" tipocampo="' + this.TipoSQL + '" value="' + this.RotuloAbreviado + '">' + this.Rotulo + '</option>';
    });
    
    if (_html != '') {
        $('#pnl-busca').show();
        $('#lst-campos-sel').append(_html);
        $('#sel-index-99000').append(_htmlSel);
    }
    FiltraOperador(4);
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
    //debugger;
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
    //debugger;
    $(jsonDataModel).each(function (_index) {

        if (this.jsonmap != 'IdDocumento' && this.jsonmap != 'PathArquivo' && this.jsonmap != 'idLote') {
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
    //debugger;
    MontarJQGrid(parseInt(jsonExecucao.idoc), strSelect, strWhere, jsonExecucao.proc, jsonDataName, jsonDataModel);
    //ResultadoPesquisa(parseInt(jsonExecucao.idoc), strSelect, strWhere, jsonExecucao.proc, jsonDataName, jsonDataModel);
}

// TODO: AndrSombra 03/11/2015
var CarregarDocumentos = function (json) {

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
    ResultadoPesquisa(parseInt(jsonExecucao.idoc), strSelect, strWhere, jsonExecucao.proc, jsonDataName, jsonDataModel);
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

// TODO: AndrSombra 03/11/2015
//var ResultadoPesquisa = function (id_documento, _campos, _where, _procedure, jsonDataName, jsonDataModel) {

//    var arr = _campos.split(',');

//    $.ajax({
//        type: 'POST',
//        dataType: 'json',
//        url: '../Consulta/AjaxCallConsultaDinamica',//action,
//        data: {
//            id_documento_modelo: parseInt(id_documento),//parseInt(jsonExecucao.idoc),
//            campos: _campos,
//            filtros: _where,
//            proc_name: _procedure,//jsonExecucao.proc
//        },
//        success: function (data) {

//            //Cabecalho
//            $(arr).each(function (i) {
//                console.log(data);
//            });

//            $(data).each(function (x, arr) {
//                //console.log(data.arr[x]);
//            });

//            /* console.log(_campos);
//             for (var c in _campos) {
//                 console.log(c);
//             }*/
//        }
//    });
//}


    var MontarJQGrid = function (id_documento, _campos, _where, _procedure, jsonDataName, jsonDataModel) {
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
            loadui: '',
            altclass: 'alt-row-class',
            caption: ''
        });

        //AndreSombra 05/11/2015
        $("#tblGrid").css('width', '100%');
        $(".ui-jqgrid-htable").css('width', '100%');

        $('th').attr('role', 'columheader').attr('class', 'ui-state-default ui-th-column ui-th-ltr ui-sortable-handle').attr('class', '').css('text-align', 'center').css('color', '#008000');
        //$('tr').attr('role', 'row').attr('class', 'ui-widget-content jqgrow ui-row-ltr').attr('class', '').attr('class', 'ui-widget-content ui-row-ltr');

        //AndreSombra 04/11/2015
        //$(".ui-jqgrid-htable").css('width', '100%');
        $(".ui-jqgrid").css('width', '100%');
        $(".ui-jqgrid-bdiv").css('width', '100%');
        //$(".ui-state-default").css('width', '100%');
        $("div[class='ui-state-default ui-jqgrid-hdiv ui-corner-top']").css('width', '100%');
        $(".ui-jqgrid-view").css('width', '100%');
        $("#tblGridpager").css('width', '100%');

       // $('#tblGrid tr').attr('role', 'row').attr('class', 'ui-widget-content jqgrow ui-row-ltr').attr('class', '').attr('class', 'ui-widget-content ui-row-ltr');

        $("#tblGrid").change(function () {
            ColunaAuto();
        });
        console.log('Montou grid');
       // $("#tblGrid tbody").change(function () {
        // });

        $("#tblGrid tr:eq(0)").remove();
        $("#gbox_tblGrid").hide();

        setTimeout(function () {
            //$("#tblGrid tr:eq(1)").remove();
            ExibirResultado();
            //$("#tblConsulta tr:eq(1)").remove();
            //$("#tblConsulta_length").hide();
            console.log('Executou clone');
        }, 1000);
    }

    function ExibirResultado() {
        //TODO: Não esta carregando os registros, somente manual no console do browse 
        //Andre Sombra 26/01/2016 

        $("#tblConsulta").empty();
        $('thead').first().clone().appendTo('#tblConsulta');
        $('#tblGrid tbody').clone().appendTo('#tblConsulta');
        
        if ($("#tblStatus").val() == 0) {
            $("#tblStatus").val(1); //Para aplicar o DataTable somente uma vez a tblConsulta
            $("#tblConsulta").DataTable({
                language: {
                    "sEmptyTable": "Nenhum registro encontrado",
                    "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
                    "sInfoFiltered": "(Filtrados de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sInfoThousands": ".",
                    "sLengthMenu": "_MENU_ resultados por página",
                    "sLoadingRecords": "Carregando...",
                    "sProcessing": "Processando...",
                    "sZeroRecords": "Nenhum registro encontrado",
                    "sSearch": "Pesquisar",
                    "oPaginate": {
                        "sNext": "Próximo",
                        "sPrevious": "Anterior",
                        "sFirst": "Primeiro",
                        "sLast": "Último"
                    },
                    "oAria": {
                        "sSortAscending": ": Ordenar colunas de forma ascendente",
                        "sSortDescending": ": Ordenar colunas de forma descendente"
                    }
                }
            });
        }

      //Esconder a coluna IdDocumento e idLote ====================================================           
        var totLinha = $('#tblConsulta tr').length;
        for (var i = 1; i < totLinha; i++) {
            $("#tblConsulta tr:eq(" + i + ") td:eq(0)").hide(); //css('background-color', 'red');
        }
        for (var i = 1; i < totLinha; i++) {
            $("#tblConsulta tr:eq(" + i + ") td:eq(1)").hide(); //css('background-color', 'green');
        }
        $("#tblConsulta th:contains('ID')").hide();
        //$(".sorting_1").hide()
        $("#tblConsulta th:contains('idLote')").hide();
        //============================================================================================

        $("#tblConsulta_info").hide();
}

    //TODO: AndreSombra 10/11/2015
    function ColunaAuto() {
        //alert('teste ColunaAuto 1');
        $('tr').attr('role', 'row').attr('class', 'ui-widget-content jqgrow ui-row-ltr').attr('class', '').attr('class', 'ui-widget-content ui-row-ltr');
    }

    function JQGrid(caption) {
        
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
                //debugger;
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

                            if (this.jsonmap != 'IdDocumento' && this.jsonmap != 'PathArquivo' && this.jsonmap != 'idLote') {
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

        //var _url = window.location.protocol + '//' + window.location.host + '/StoragePrivate/' + cellvalue;
        //var _ret = '<center><table border=0 cellspacing=10 cellpadding=10 ><tr><td> ';
        //_ret +='<a href=' + _url + ' class="ls-btn-primary" target="_blank" style="target-new: tab;target-new: tab;">Visualizar</a>';
        //_ret += ' </td><td> ';
        //_ret += '<a id="btn_Editar" class="ls-btn-primary" href="/Documento/Digitacao/Digitar/' + rowObject.IdDocumento + '">Editar</a> ';
        //_ret += '</td></tr><table></center>'

        var _url = window.location.protocol + '//' + window.location.host + '/StoragePrivate/' + cellvalue;
        var _ret = '';
        _ret += '<a href=' + _url + ' class="ls-btn-primary" target="_blank" style="target-new: tab;target-new: tab;"><span class="glyphicon glyphicon-picture"></span></a>&nbsp;&nbsp;';
        _ret += '<a id="btn_Editar" class="ls-btn-primary" href="/Documento/Digitacao/Digitar/' + rowObject.IdDocumento + '"><span class="glyphicon glyphicon-pencil"></span></a> ';
        _ret += '<a id="btn_documentos" class="ls-btn-primary" href="#" onclick="AbreSubDocumentos(' + rowObject.IdDocumento + ','+ rowObject.idLote +')"><i class="fa fa-plus"></i></a> ';

        return _ret;

    }

    function AbreSubDocumentos(v_idDoc, v_idLote) {
        //alert('Documento: ' + v_idDoc);
        $("#pnl_parametros").hide();
        $("#pnl_resultado").hide();
        $("#pnl_resultado_detalhe").show();
        var _url = window.location.protocol + '//' + window.location.host + '/StoragePrivate/';
        //var _img_det = '';
        //_img_det += '<a href=' + v_url + ' class="ls-btn-primary" target="_blank" style="target-new: tab;target-new: tab;"><span class="glyphicon glyphicon-picture"></span></a>&nbsp;&nbsp;';

        //var table_detalhe = $("#tblDetalhe").DataTable();
        //table_detalhe.destroy();

        $("#tblDetalhe").show();
        
        $.ajax({
            url: "/Documento/Consulta/AjaxCallConsultaDetalhe",
            type: 'POST',
            datatype: 'json',
            data: { idDoc: v_idDoc, idLote: v_idLote },
            success: function (data) {
                //debugger;
                //_url =  + item.PathArquivo;
                if (data == null) {
                    exibirmsgatencao("Nenhum documento encontrado.");
                    return;
                }
                
                $("#tblDetalhe tbody").empty();
                
                $.each(data, function (i, item) {
                  //$("#tblDetalhe, tbody").append(item.Descricao);
                  $("#tblDetalhe tbody").append("<tr><td>" + item.Descricao + "</td><td><a href='" + _url + item.PathArquivo + "' class='ls-btn-primary' target='_blank' style='target-new: tab;target-new: tab;'><span class='glyphicon glyphicon-picture'></span></a>&nbsp;&nbsp;</td></tr>");
                });

                AplicarDataTable();
                $("#tblDetalhe_info").hide();
            },
        });

        $("#tblDetalhe").hide();
        $.ajax({
            url: "/Documento/Consulta/AjaxListaDetalhe",
            type: 'POST',
            datatype: 'html',
            data: { idDoc: v_idDoc, idLote: v_idLote },
            success: function (data) {
                $("#lista_detalhe").empty();
                $("#lista_detalhe").html(data);
            },
        });

       //$("#tblDetalhe_length").hide(); //Ocultar Quantidade de paginas ,25,50...
    }

    function AplicarDataTable()
    {
        if ($("#tblStatus_detalhe").val() == 0) {
            $("#tblStatus_detalhe").val(1); //Para aplicar o DataTable somente uma vez a tblConsulta
            //$("#tblDetalhe").DataTable({
            //    language: {
            //        "sEmptyTable": "Nenhum registro encontrado",
            //        "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            //        "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            //        "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            //        "sInfoPostFix": "",
            //        "sInfoThousands": ".",
            //        "sLengthMenu": "_MENU_ resultados por página",
            //        "sLoadingRecords": "Carregando...",
            //        "sProcessing": "Processando...",
            //        "sZeroRecords": "Nenhum registro encontrado",
            //        "sSearch": "Pesquisar",
            //        "oPaginate": {
            //            "sNext": "Próximo",
            //            "sPrevious": "Anterior",
            //            "sFirst": "Primeiro",
            //            "sLast": "Último"
            //        },
            //        "oAria": {
            //            "sSortAscending": ": Ordenar colunas de forma ascendente",
            //            "sSortDescending": ": Ordenar colunas de forma descendente"
            //        },
            //    }
            //});
        }

        var table_detalhe = $('#tblDetalhe').DataTable();
        setInterval(function () {
           // table_detalhe.ajax.reload();
        }, 1000);
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
        console.log(_cn.toString());
        return _cn;
    }
    function columnsDataModel(json) {
        var _cm = '{}';
        $(json).each(function () {
            _cm = this;
            //_html += monta_input(this.Rotulo, 'pesq_' + this.RotuloAbreviado + '_' + this.ID, this.ControleWEB, this.Rotulo, this.Descricao, this.Descricao, this.MaxLength, this.Requerido, this.ClasseCSS, this.Tabulacao);
        });
        console.log(_cm.toString());
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

        _strTH += '"idLote", ';
        _strTR += '{ name: "idLote", index: "idLote", jsonmap: "idLote", align: "center", width: 20},';

        $('#lst-campos-sel li').each(function () {
            _strTH += '"' + this.innerText + '", ';
            _strTR += '{ name: "' + this.innerText + '",    index: "' + this.id + '",     jsonmap: "' + this.id + '",   align: "center",  width: 20},';
        });

        _strTR = _strTR + '{ name: "PathArquivo",  index: "PathArquivo",   jsonmap: "PathArquivo", align: "center",      width: 25, sortable: false, formatter: "#FormatterLinkOpenArquivo#"}], ';
        _strTH = _strTH + '"Arquivo"],';

        var _strWHERE = '"where": [';

        $('div[id^="pnl-filtro"]').each(function () {
            var _id = '';
            var _mascara = '';
            _id = this.id.replace('pnl-filtro-f-', '');
            _id = _id.replace('pnl-filtro-d-', '');
            
            //TODO: AndreSombra 12/11/2015
            _mascara = $('#sel-index-' + _id + ' option:selected').attr("mascara")
            var _mascarasaida = $('#sel-index-' + _id + ' option:selected').attr("mascarasaida")
            $('#txtValor-' + _id).mask(_mascarasaida);  //Colocar MascaraSaida

            //TODO: AndreSombra 13/11/2015
            if ($('#sel-operador-' + _id + ' option:selected').val() == 'Like') {
                var _valor = '%' + $('#txtValor-' + _id).val() + '%';
                _strWHERE += '{ sel: "' + $('#sel-index-' + _id + ' option:selected').val() + '", op: "' + $('#sel-operador-' + _id + ' option:selected').val() + '", val: "\'' + _valor + '\'", x: "' + $('#sel-condicao-' + _id + ' option:selected').val() + '" },'
            }
            else {
                _strWHERE += '{ sel: "' + $('#sel-index-' + _id + ' option:selected').val() + '", op: "' + $('#sel-operador-' + _id + ' option:selected').val() + '", val: "\'' + $('#txtValor-' + _id).val() + '\'", x: "' + $('#sel-condicao-' + _id + ' option:selected').val() + '" },'
            }

        });
        
        _strWHERE = _strWHERE + '],';
        _strEXEC = '"exec":{proc:"proc_consulta_dinamica_padrao", idoc: "' + $("#cboTiposDoc option:selected").val() + '", par:""}';

        //alert(_strWHERE.toString());

        _str = _str + _strTH + _strTR + _strWHERE + _strEXEC + '}';
        
        //_str = _str + _strTH + _strTR   + '}';

        //TODO: AndreSombra 12/11/2015
        //Colocar MascaraEntrada
        $('div[id^="pnl-filtro"]').each(function () {
            var _id = '';
            var _mascara = '';
            _id = this.id.replace('pnl-filtro-f-', '');
            _id = _id.replace('pnl-filtro-d-', '');

            _mascara = $('#sel-index-' + _id + ' option:selected').attr("mascara")
            var _mascara = $('#sel-index-' + _id + ' option:selected').attr("mascara")
            $('#txtValor-' + _id).mask(_mascara);
        });
       // debugger;
        console.log(_str.toString());
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
        console.log(_str.toString());
        return str;
    }
    //------end grid part----------
    function NewGuid() {
        var sGuid = "";
        for (var i = 0; i < 32; i++) {
            sGuid += Math.floor(Math.random() * 0xF).toString(0xF);
        }
        return sGuid;
    }

    var AdicionaCamposFiltro = function (data) {

        var _id = NewGuid();
        _id = _id.substr(15, 5);

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
        _html += '              <select id="sel-index-' + _id + '" class="form-control" onchange="AplicarMascara(this)">'; //TODO: AndreSombra 11/11/2015
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
        _html += '                  <option value="Like"> Contém </option>';
        _html += '                  <option value="!="> Diferente </option>';
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
        
        //TODO: AndreSombra 13/11/2015
        var _tipocampo = $("#sel-index-" + _id + " option:selected").attr("tipocampo");
        $("#sel-operador-" + _id).each(function () {
            //alert(seloper);
            if (_tipocampo == "A") {

                $("#sel-operador-" + _id + " option[value='Like']").css('display', '');
                $("#sel-operador-" + _id + " option[value='!=']").css('display', 'none');
                $("#sel-operador-" + _id + " option[value='>']").css('display', 'none');
                $("#sel-operador-" + _id + " option[value='<']").css('display', 'none');
            } else {

                $("#sel-operador-" + _id + " option[value='Like']").css('display', 'none');
                $("#sel-operador-" + _id + " option[value='!=']").css('display', '');
                $("#sel-operador-" + _id + " option[value='>']").css('display', '');
                $("#sel-operador-" + _id + " option[value='<']").css('display', '');
            }
        });
    }
    
    //TODO: AndreSombra 11/11/2015
    function AplicarMascara(campo) {
        var cmp = $("#" + campo.id.replace('sel-index-', 'txtValor-'));
        var _mascara = $("#" + campo.id + " option:selected").attr("mascara");
        if (_mascara != '') {
            cmp.val('');
            cmp.mask(_mascara)
        }
        else {
            cmp.val('');
            cmp.unmask();
        }
        //FiltraOperador(5);
        FiltraOperadorDinamico(campo);
    }

    function FiltraOperador(num) {
        //TODO: AndreSombra 12/11/2015
        var _tipocampo = $("#sel-index-99000 option:selected").attr("tipocampo");
        //alert('Chamou FiltroOperador : ' + _tipocampo + " Numero: " + num);
        $("#sel-operador-99000").each(function () {
            if (_tipocampo == "A") {
                $("#sel-operador-99000 option[value='Like']").css('display', '');
                $("#sel-operador-99000 option[value='!=']").css('display', 'none');
                $("#sel-operador-99000 option[value='>']").css('display', 'none');
                $("#sel-operador-99000 option[value='<']").css('display', 'none');
            } else {
                $("#sel-operador-99000 option[value='Like']").css('display', 'none');
                $("#sel-operador-99000 option[value='!=']").css('display', '');
                $("#sel-operador-99000 option[value='>']").css('display', '');
                $("#sel-operador-99000 option[value='<']").css('display', '');
            }
        });
    }

    function FiltraOperadorDinamico(campo) {
        //TODO: AndreSombra 12/11/2015
        var _id = campo.id;
        _id = _id.replace('sel-index-', 'sel-operador-');
        //alert(_id);
        var _tipocampo = $("#" + campo.id + " option:selected").attr("tipocampo");
        var seloper = $("#" + _id);
        //alert(seloper);
        //alert('Chamou FiltroOperador : ' + _tipocampo+" campoid= "+campo.id);
        seloper.each(function () {
            //alert(seloper);
            if (_tipocampo == "A") {
                
                $("#" + _id + " option[value='Like']").css('display', '');
                $("#" + _id + " option[value='!=']").css('display', 'none');
                $("#" + _id + " option[value='>']").css('display', 'none');
                $("#" + _id + " option[value='<']").css('display', 'none');
            } else {
                
                $("#" + _id + " option[value='Like']").css('display', 'none');
                $("#" + _id + " option[value='!=']").css('display', '');
                $("#" + _id + " option[value='>']").css('display', '');
                $("#" + _id + " option[value='<']").css('display', '');
            }
        });
    }
