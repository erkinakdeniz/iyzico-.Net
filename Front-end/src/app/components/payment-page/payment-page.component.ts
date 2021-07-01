import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { CartItem } from 'src/app/models/cartItem';
import { PaymentCard } from 'src/app/models/paymentCard';
import { Product } from 'src/app/models/product';
import { PayService } from 'src/app/services/pay.service';

@Component({
  selector: 'app-payment-page',
  templateUrl: './payment-page.component.html',
  styleUrls: ['./payment-page.component.css'],
})
export class PaymentPageComponent implements OnInit {
  paymentCard!: PaymentCard;
  totalprice!: string;
  cartItem!: CartItem;
  cartItems: CartItem[] = [];
  product!: Product;
  constructor(
    private formBuilder: FormBuilder,
    private payService: PayService
  ) {}
  paymentAddForm!: FormGroup;
  ngOnInit(): void {
    this.CreatePaymentAddForm();
    this.CartItemm();
  }
  CreatePaymentAddForm() {
    this.paymentAddForm = this.formBuilder.group({
      firstName: [''],
      lastName: [''],
      email: [''],
      Identification: [''],
      address: [''],
      country: [''],
      state: [''],
      same_address: [''],
      save_info: [''],
      paymentMethod: [''],
      cc_name: [''],
      cc_number: [''],
      cc_expiration: [''],
      cc_cvv: [''],
      registerCard: [''],
    });
  }
  CartItemm() {
    this.cartItems = [];
    this.product = new Product();
    this.product.brandId = '1';
    this.product.description = 'sdfs';
    this.product.productID = 4;
    this.product.productName = 'defter';
    this.product.subCategoryId = 3;
    this.product.unitPrice = 10;
    this.product.unitsInStock = 200;

    this.cartItem = new CartItem();
    this.cartItem.product = this.product;
    this.cartItem.quantity = 20;
    this.cartItems.push(this.cartItem);
    
    this.totalprice = String(this.product.unitPrice * this.cartItem.quantity);
  }
  write() {
    this.paymentCard = new PaymentCard();
    let ExpireMonth = (<HTMLInputElement>(
      document.getElementById('kt_inputmask_1')
    )).value.substring(0, 2);
    let ExpireYear = (<HTMLInputElement>(
      document.getElementById('kt_inputmask_1')
    )).value.substring(2, 4);
    let CardNumber = (<HTMLInputElement>(
      document.getElementById('kt_inputmask_credit')
    )).value;
    let paymentForms = Object.assign({}, this.paymentAddForm.value);
    
    this.paymentCard.CardHolderName = paymentForms.cc_name;
    this.paymentCard.CardNumber = CardNumber;
    this.paymentCard.City = paymentForms.state;
    this.paymentCard.ContactName = paymentForms.firstName;
    this.paymentCard.Country = paymentForms.country;
    this.paymentCard.Cvc = paymentForms.cc_cvv;
    this.paymentCard.Email = paymentForms.email;
    this.paymentCard.ExpireMonth = ExpireMonth;
    this.paymentCard.ExpireYear = ExpireYear;
    this.paymentCard.IdentityNumber = paymentForms.Identification;
    this.paymentCard.Name = paymentForms.firstName;
    this.paymentCard.Price = this.totalprice;
    this.paymentCard.RegisterCard = 0;
    this.paymentCard.RegistrationAddress = paymentForms.address;
    this.paymentCard.Surname = paymentForms.lastName;
    this.paymentCard.cartItems = this.cartItems;
    console.log(paymentForms);
    console.log(this.paymentCard);
    console.log(this.cartItems);
    this.payService.addPay(this.paymentCard).subscribe((response) => {
      console.log(response.message);
    },errorResponse=>{console.log(errorResponse.error.message)});
  }
}
