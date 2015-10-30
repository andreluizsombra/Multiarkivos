
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