/* JAVASCRIPT UTILS
--------------------------------------------------------------*/
// Método utilizado para a recuperação do nome do método de chamada
function GetMethodName(callee) {
    // arguments.callee.name; // FF, Chrome, Opera, Safari
    // arguments.callee.toString(0).match(/^function\s*(?:\s+([\w\$]*))?\s*\(/)[1]; // IE    
    if (callee == null || callee == undefined) { return null; }
    return (callee.name == undefined ? callee.toString(0).match(/^function\s*(?:\s+([\w\$]*))?\s*\(/)[1] : callee.name);
}

// Método de formação date para um formato específico ('dd/mm/yyyy', 'm/d/y' ...)
// Depende do date.format.js
String.prototype.toDateString = function (_format) {
    if (!this) { return new String(); }
    var _value = this;

    if (_value.replace(/\D/g, '') == 62135589600000) return "";
    var _date = new Date(+_value.replace(/\D/g, ''));

    return dateFormat(_date, _format);
}

// Método similar ao String.Format do .NET ('Exemplo nota {0}'.format('10'))
String.prototype.format = function () {
    if (!this) { return new String(); }
    var s = this, i = arguments.length;

    while (i--) { s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]); }
    return s;
};

// Método que finaliza se a string finaliza com a sufixo informado
String.prototype.endsWith = function (suffix) {
    if (!this) { return false; }
    return (this.substr(this.length - suffix.length) === suffix);
}

// Método que verifica se a string inicia com o prefixo informado
String.prototype.startsWith = function (prefix) {
    if (!this) { return false; }
    return (this.substr(0, prefix.length) === prefix);
}

// Método que replica a String informada de acordo com a quantidade de vezes estabelecida
String.prototype.repeat = function (num) {
    if (!this) { return new String(); }
    return new Array(num + 1).join(this);
}

// Método similar ao String.IsNullOrEmpty do .NET ("".isNullOrEmpty(); teste.isNullOrEmpty())
String.prototype.isNullOrEmpty = function () {
    if (!this) { return true; }
    return (this === null || this.length == 0 || this === undefined);
}

// Método similar ao String.IsNullOrWhiteSpace do .NET ("".isNullOrWhiteSpace(); teste.isNullOrWhiteSpace())
String.prototype.isNullOrWhiteSpace = function () {
    if (!this) { return true; }
    return (this === null || this.trim().length == 0 || this === undefined);
}


String.prototype.toFloat = function () {

    if (this === null || this.trim().length == 0 || this === undefined) {
        return 0;
    }

    var result = this.replace('.', '').replace(',', '.');
    if (isNaN(result)) {
        return 0;
    } else {
        return parseFloat(result);
    }

}

Number.prototype.toFloat = function () {

    if (this === null || this === undefined || isNaN(this)) {
        return 0;
    }
    return this.toString().replace('.', ',');
}

// Método que realiza comparação em um periodo e verifica a diferença entre as datas Ex.: DateDiff.inDays(d1,d2) > 20
DateDiffPlugin = function ()
{ this.init(); }

$.extend(DateDiffPlugin.prototype, {
    init: function ()
    { },
    inDays: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000));
    },
    inWeeks: function (d1, d2) {
        var t2 = d2.getTime();
        var t1 = d1.getTime();

        return parseInt((t2 - t1) / (24 * 3600 * 1000 * 7));
    },
    inMonths: function (d1, d2) {
        var d1Y = d1.getFullYear();
        var d2Y = d2.getFullYear();
        var d1M = d1.getMonth();
        var d2M = d2.getMonth();

        return (d2M + 12 * d2Y) - (d1M + 12 * d1Y);
    },
    inYears: function (d1, d2) {
        return d2.getFullYear() - d1.getFullYear();
    }
})
// Instância default do DateDiffPlugin
var DateDiff = new DateDiffPlugin();