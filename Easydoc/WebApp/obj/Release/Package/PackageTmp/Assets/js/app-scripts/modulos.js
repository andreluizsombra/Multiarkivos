function VerificarAcessoModulo(idModulo) {
    $.ajax({
        url: '/Home/VisualizarModulo',
        type: 'POST',
        data: { _idModulo: idModulo },
        success: function (data) {
            //alert(data.rest);
            $.each(data, function () {
                //debugger;
                //console.log(this.idModulo);
                this.Habilitado == '1' ? $("#modulo_" + this.idModulo).show() : $("#modulo_" + this.idModulo).hide();
            });
        }
    });
}

function VerificarAcessoModuloPrincipal() {
    $.ajax({
        url: '/Home/VisualizarModuloPrincipal',
        type: 'POST',
        success: function (data) {
            $.each(data, function () {
                this.Habilitado == '1' ? $("#modulo_" + this.idModulo).show() : $("#modulo_" + this.idModulo).hide();
            });
        }
    });
}