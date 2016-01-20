jQuery(document).ready(function () { init(); });

var init = function () {
    
}

var ajax_listar_supervisao = function () { 

    try {
        $.ajax({
            url: '../Consulta/AjaxCallListarPendentesSupervisao',
            dataType: 'json',
            type: 'POST',
            beforeSend: function (xhr) { $.blockUI(blockUISettings); },
            data: {},
            success: function (data, textstatus, xmlhttprequest) {
                if (data == null) { return; }

                if (data.success == true) {
                    data = data.output;

                    var _html = '';

                    if (data != '') {
                        _html += '<table class="ls-table ls-bg-header ls-table-striped">';
                        _html += '<thead>';
                        _html += '<tr>';
                        _html += '<th>ID</th>';
                        _html += '<th class="hidden-xs">Matricula</th>';
                        _html += '<th class="hidden-xs">Arquivo</th>';
                        _html += '</tr>';
                        _html += '</thead>';

                        $(data).each(function () {
                            _html += '<tr>';
                            _html += '<td class="hidden-xs">' + this.IdDocumento + '</td>';
                            _html += '<td class="hidden-xs">' + this.Matricula + '</td>';
                            var _url = window.location.protocol + '//' + window.location.host + '/StoragePrivate/' + this.PathArquivo;
                            _html += '<td><a href="' + _url + '" title="Arquivo do docuemnto" target="_blank" style="target-new: tab;target-new: tab;">Vizualizar.</a></td>';
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