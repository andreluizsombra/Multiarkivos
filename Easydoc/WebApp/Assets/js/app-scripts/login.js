$(document).ready(function () {
    $('a.trocar_servico_atual')
        .bind('click', function () {
            var id_servico = $(this).attr('id').replace('serv_','');
            TrocarServicoAtual(id_servico);
        });
    TrocarServicoAtual(0);

    //locastyle.dismiss.init({ target: '#AlertaLogin' });

    //locastyle.dismiss.init();
    return true;
});