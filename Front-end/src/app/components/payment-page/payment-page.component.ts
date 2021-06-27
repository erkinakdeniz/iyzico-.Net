import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-payment-page',
  templateUrl: './payment-page.component.html',
  styleUrls: ['./payment-page.component.css'],
})
export class PaymentPageComponent implements OnInit {
 
  constructor(private formBuilder: FormBuilder) {
   
  }
  paymentAddForm!: FormGroup;
  ngOnInit(): void {
    this.CreatePaymentAddForm();
  }
  CreatePaymentAddForm() {
    this.paymentAddForm=this.formBuilder.group({
      asd:[''],
    });
  }
 
  write()
  {
    
    let paymentForms=Object.assign({},this.paymentAddForm.value);
  
   let expiration=(<HTMLInputElement>document.getElementById("kt_inputmask_1")).value.substring(0,2);
   //this.Expiration_year=(<HTMLInputElement>document.getElementById("asd")).value.substring(2,4);
   console.log(expiration);
   paymentForms.asd=expiration;
   console.log(paymentForms);
   
   
  }
}
