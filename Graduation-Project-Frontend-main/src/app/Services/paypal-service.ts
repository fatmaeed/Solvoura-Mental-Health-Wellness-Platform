import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PaypalService {
  private isScriptLoaded = false;

  loadPaypalScript(clientId: string): Promise<void> {
    return new Promise((resolve, reject) => {
      if (this.isScriptLoaded) {
        resolve();
        return;
      }

      const script = document.createElement('script');
      script.src = `https://www.paypal.com/sdk/js?client-id=${clientId}&currency=USD`;
      script.onload = () => {
        this.isScriptLoaded = true;
        resolve();
      };
      script.onerror = () => reject();
      document.head.appendChild(script);
    });
  }
}
