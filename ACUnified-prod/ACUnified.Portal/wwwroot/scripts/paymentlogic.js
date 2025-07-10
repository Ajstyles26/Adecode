function makePayment(args){
    //console.log(args);
    var payParams=JSON.parse(args)
    //console.log(payParams.TranId);
    return new Promise((mainResolve, mainReject) => {
    if(payParams.PayGateway===1){
        var paymentEngine = RmPaymentEngine.init({
            key: payParams.prodKey,
            transactionId: payParams.TranId,
            processRrr: true,
            //customerId: payParams.CustomerId,
            //firstName: payParams.FirstName,
            //lastName: payParams.LastName,
            //email: payParams.Email,
            //amount: payParams.TotalAmount,
            //narration: payParams.TranId,
            channel: "CARD,PAYWITHREMITA,BANK,BRANCH,IBANK",
            extendedData: { // Optional field. Details are available in the table
                customFields: [{
                    name: "rrr",
                    value: payParams.RRR
                }],
               
            },
            onSuccess: function (response) {

                var myHeaders = new Headers();
                myHeaders.append("Content-Type", "application/json");
                myHeaders.append("Authorization", "remitaConsumerKey=" + payParams.apiKey + ",remitaConsumerToken="+payParams.apiHash+"");

                var requestOptions = {
                    method: 'GET',
                    headers: myHeaders,
                    redirect: 'follow'
                };

                fetch("https://login.remita.net/remita/exapp/api/v1/send/api/echannelsvc/"+payParams.apiKey+"/" + payParams.TranId +"/"+payParams.apiHash+"/orderstatus.reg", requestOptions)
                    .then(response => response.text())
                    .then(result => console.log("result is" + result))
                    .catch(error => console.log('error', error));
                mainResolve({
                    success: true, message: "Payment processed successfully",
                    payreference:  payParams.TranId
                });
            
            

            },
            onError: function (response) {

                var myHeaders = new Headers();
                myHeaders.append("Content-Type", "application/json");
                myHeaders.append("Authorization", "remitaConsumerKey=" + payParams.apiKey + ",remitaConsumerToken=" + payParams.apiHash + "");

                var requestOptions = {
                    method: 'GET',
                    headers: myHeaders,
                    redirect: 'follow'
                };

                fetch("https://login.remita.net/remita/exapp/api/v1/send/api/echannelsvc/" + payParams.apiKey + "/" + payParams.TranId + "/" + payParams.apiHash + "/orderstatus.reg", requestOptions)
                    .then(response => response.text())
                    .then(result => console.log("result is"+result))
                    .catch(error => console.log('error', error));

                mainResolve({
                    success: false, message: "Payment pending",
                    payreference:  payParams.TranId
                });

            },
            onClose: function () {
                var myHeaders = new Headers();
                myHeaders.append("Content-Type", "application/json");
                myHeaders.append("Authorization", "remitaConsumerKey=" + payParams.apiKey + ",remitaConsumerToken=" + payParams.apiHash + "");

                var requestOptions = {
                    method: 'GET',
                    headers: myHeaders,
                    redirect: 'follow'
                };

                fetch("https://login.remita.net/remita/exapp/api/v1/send/api/echannelsvc/" + payParams.apiKey + "/" + payParams.TranId + "/" + payParams.apiHash + "/orderstatus.reg", requestOptions)
                    .then(response => response.text())
                    .then(result => console.log("result is" + result))
                    .catch(error => console.log('error', error));

                mainResolve({
                    success: false, message: "Payment pending",
                    payreference: payParams.TranId
                });
                mainResolve({
                    success: false, message: "Payment terminated",
                    payreference:  payParams.TranId
                });
                

            }
        });
        paymentEngine.showPaymentWidget();
    }
    else{
        console.log('Paystack...');

        let handler = new PaystackPop();
        handler.newTransaction({
            key: 'pk_test_68ca186c03ac7b60fa8ba65cb74d44eb449f2657',
            email: payParams.Email,
            amount: payParams.TotalAmount*100,
            ref:payParams.TranId  ,
           // ref:payParams.CustomerId + "" + payParams.TranId + "" + payParams.SelectedItems,
            onClose: function () {
                mainResolve({
                    success: false, message: "Payment failure",
                    payreference:  payParams.TranId
                });
            },
            callback: function (response) {




                mainResolve({
                    success: true, message: "Payment processed successfully",
                    payreference:  payParams.TranId
                });
                
            },
            onError: function (response) {

                mainResolve({
                    success: false, message: "Payment failure",
                    payreference:  payParams.TranId
                });

            },
        });
    }
    });
}