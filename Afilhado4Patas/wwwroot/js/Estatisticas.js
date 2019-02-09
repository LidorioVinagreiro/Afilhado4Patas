

function getApadrinhamentos() {
        $.ajax({
            type: "GET",
            url: "/Funcionario/apadrinhamentosPorMes",
            dataType: "json",
            //data: { model: JSON.stringify(usersRoles) },
            success: function(data) { return data; },
            failure: function(errMsg) {
                alert(errMsg);
            }
        });
    }

function getAdocoes() {
    $.ajax({
        type: "GET",
        url: "/Funcionario/adocoesPorMes",
        dataType: "json",
        //data: { model: JSON.stringify(usersRoles) },
        success: function (data) { return data; },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getTiposAnimais() {
    $.ajax({
        type: "GET",
        url: "/Funcionario/PieAnimal",
        dataType: "json",
        //data: { model: JSON.stringify(usersRoles) },
        success: function (data) { return  data; },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}   

function getSexoUtilizadores() {
    $.ajax({
        type: "GET",
        url: "/Funcionario/PieMf",
        dataType: "json",
        //data: { model: JSON.stringify(usersRoles) },
        success: function (data) { return data; },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getNumeroPedidosPasseio() {
    $.ajax({
        type: "GET",
        url: "/Funcionario/numeroPedidosPasseioPorMes",
        dataType: "json",
        //data: { model: JSON.stringify(usersRoles) },
        success: function (data) { return data; },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getNumeroPedidosFinsSemana() {
    $.ajax({
        type: "GET",
        url: "/Funcionario/numeroPedidosFdsPorMes",
        dataType: "json",
        //data: { model: JSON.stringify(usersRoles) },
        success: function (data) { return data; },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}