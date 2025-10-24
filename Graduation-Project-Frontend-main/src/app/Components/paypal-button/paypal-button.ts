// components/paypal-button/paypal-button.component.ts
import { ChangeDetectorRef, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PaypalService } from '../../Services/paypal-service';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { PaymentService } from '../../Services/payment-service';
import { ICreatePayment } from '../../Models/icreate-payment';

declare var paypal: any;

@Component({
  selector: 'app-paypal-button',
  template: '<div id="paypal-button-container"></div>',
  styles: ['#paypal-button-container { width: 250px; margin: 0 auto; }']
})
export class PaypalButton implements OnInit {
showAlert = false;
alertMessage = '';



  @Input() amount!: number ;
  @Input() description: string = 'Default Description';
paymentId!:number ;
  constructor(private paypalService: PaypalService , private http:HttpClient , private paymentService:PaymentService) {}

  ngOnInit(): void {
    this.getExchangeRateAndInit() ;
    this.paypalService.loadPaypalScript('AVbmz7OZ3vVv-tJbOJedXcKRQmPzJICJe0jFik1bqkSgzmYQXJOrYRXpxWTyCfxXCJ71ozAO2WUliAHH').then(() => {
      this.initConfig();
   });

  }


 usdAmount!: string;
  getExchangeRateAndInit() {
    this.http.get<any>('https://v6.exchangerate-api.com/v6/20d34d34f5ee0742dfab3100/pair/EGP/USD')
      .subscribe(response => {
        const rate = response.conversion_rate;
        if (!rate) {
          this.showCustomAlert('Failed to get USD exchange rate');
          return;
        }
         this.usdAmount = (this.amount * rate).toFixed(2);


      });
  }




showCustomAlert(message: string): void {
  this.alertMessage = message;
  this.showAlert = true;
}


  private initConfig(): void {
  paypal.Buttons({
    createOrder: (data: any, actions: any) => {
      return actions.order.create({
        intent: 'CAPTURE',
        purchase_units: [{
          amount: {
            currency_code: 'USD',
            value: this.usdAmount.toString(),
            breakdown: {
              item_total: {
                currency_code: 'USD',
                value: this.usdAmount.toString()
              }
            }
          },
          items: [{
            name: this.description,
            unit_amount: {
              value:this.usdAmount.toString(),
              currency_code: 'USD'
            },
            quantity: '1'
          }]
        }]
      });
    },
    onApprove: (data: any, actions: any) => {
      console.log('onApprove - transaction approved', data);
      actions.order.get().then((details: any) => {
        console.log('Order details: ', details);


        this.showCustomAlert(`Transaction completed by ${details.payer.name.given_name}`);
        let payment:ICreatePayment =
         {transactionId:data.paymentID , paymentMethod:data.paymentSource
           , paymentDate:details.create_time , status:details.status , amount:this.amount}

        this.paymentService.createPayment(payment).subscribe({
        next: (response) => {
          this.paymentId = response.paymentId ;
          console.log('payment id in DB', response.paymentId);
          this.sendData() ;

         },
        error: (error) => { console.error('Error creating Payment:', error); }
      });
      });
    },
    onClientAuthorization: (data: any) => {
      console.log('onClientAuthorization', data);
      this.showCustomAlert('Transaction completed by ' + data.payer.name.given_name);
    },
    onCancel: (data: any) => {
      console.log('Transaction cancelled', data);
    },
    onError: (err: any) => {
      console.error('PayPal error', err);
      this.showCustomAlert('An error occurred during the transaction.');
    },
    style: {
      layout: 'vertical',
      label: 'paypal'
    }
  }).render('#paypal-button-container');
}

 @Output() dataEmitter = new EventEmitter<{}>();

  sendData() {

    let data =  {isPaid:true ,paymentId: this.paymentId}
    this.dataEmitter.emit(data);
  }


onAlertClosed() {
  this.showAlert = false;
}


}
