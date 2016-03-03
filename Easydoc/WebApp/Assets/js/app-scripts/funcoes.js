var ConverteData = function (data) {

    if (data == '') { return "-" }
    var _ret = '';

    _ret = data.substr(6, 2) + '/' + data.substr(4, 2) + '/' + data.substr(0, 4);

    return _ret;
}
function IrTopo() {
    $('html, body').animate({
        scrollTop: 0
    }, 1000, 'easeInOutCirc');
}
function RetMascara(campo, mascara) {
    $("#" + campo.id).mask(mascara);
    //alert('ok');
}

function exibirmsg(txtmsg) {
   // $('#msg').html("<i class='glyphicon glyphicon-ok'></i>&nbsp;" + txtmsg + "<button type='button' id='btnfechar' class='close' aria-label='Close'><span >&times;</span></button>").slideDown(1000);
   // $('#btnfechar').click(function () {
   //     $('#msg').slideUp(1000);
   // });

    $("#modalMSG").modal('show');
    $("#lblmsg").text(txtmsg);
}

function exibirmsgatencao(txtmsg) {
    //$('#msgatencao').html("<i class='glyphicon glyphicon-ok'></i>&nbsp;<strong>Atenção,&nbsp;</strong>" + txtmsg + "<button type='button' id='btnfecharatencao' class='close' aria-label='Close'><span >&times;</span></button>").slideDown(1000);
    //$('#btnfecharatencao').click(function () {
    //    $('#msgatencao').slideUp(1000);
    //});

    $("#modalERRO").modal('show');
    $("#lblmsg_erro").text(txtmsg);
}

function PaginarTable(nmTable) {
    $("#"+nmTable).DataTable({
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
function validar(campo, msg) {
    var cmp = $('#' + campo);

    $('form').submit(function () {
        if (cmp.val() == '') {
            cmp.after("<span class='msgvalida text-danger'><i class='fa fa-exclamation-circle'></i>&nbsp;" + msg + "</span>").slideDown(8800);
            //$('form').after("<div class='divcentro'><div class='row'>" + msg + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i id='btn_fechar' class='fa fa-close'></i></div></div>").slideDown(8800);
            cmp.css('border-color', 'red');
            setTimeout(function () {
                $('.divcentro').remove();
                $('.msgvalida').remove();
            }, 3000);
            $("#btn_fechar").click(function () {
                $('.divcentro').remove();
                $('.msgvalida').remove();
                cmp.focus();
            });

            return false;
        } else {
            cmp.css('border-color', '');
        }
        return true;
    });

}

function DataAtual() {
    var date = new Date();
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();
    if (month < 10) month = "0" + month;
    if (day < 10) day = "0" + day;
    var today = year + "-" + month + "-" + day; //Retorna nesse formato: yyyy-MM-dd 
    return today;
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