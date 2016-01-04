function RetMascara(campo, mascara) {
    $("#" + campo.id).mask(mascara);
    //alert('ok');
}

function exibirmsg(txtmsg) {
    $('#msg').html("<i class='glyphicon glyphicon-ok'></i>&nbsp;" + txtmsg + "<button type='button' id='btnfechar' class='close' aria-label='Close'><span >&times;</span></button>").slideDown(1000);
    $('#btnfechar').click(function () {
        $('#msg').slideUp(1000);
    });
}

function exibirmsgatencao(txtmsg) {
    $('#msgatencao').html("<i class='glyphicon glyphicon-ok'></i>&nbsp;<strong>Atenção,&nbsp;</strong>" + txtmsg + "<button type='button' id='btnfecharatencao' class='close' aria-label='Close'><span >&times;</span></button>").slideDown(1000);
    $('#btnfecharatencao').click(function () {
        $('#msgatencao').slideUp(1000);
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