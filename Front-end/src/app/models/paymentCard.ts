import { CartItem } from "./cartItem";

export class PaymentCard{
    CardHolderName!: string;
    CardNumber!: string;
    ExpireMonth!: string;
    ExpireYear!: string;
    Cvc!: string;
    RegisterCard!: number;
    Name!: string;
    Surname!: string;
    Email!: string;
    IdentityNumber!: string;
    RegistrationAddress!: string;
    City!: string;
    Country!: string;
    ContactName!: string;
    Price!: string;
    cartItems!: CartItem[];
}