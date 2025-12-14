import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { CreateAddressDto } from "../../interfaces/address-interface";
import { Box } from "../../interfaces/box-inteface";
import { CreateCustomerDto } from "../../interfaces/customer-interface";
import { OrderCreateDto } from "../../interfaces/order-interface";
import { BoxService } from "../../services/box-service";
import { OrderService } from "../../services/order-service";

@Component({
  selector: 'create-box',
  templateUrl: './createorder.component.html',
})
export class CreateorderComponent {
  order: OrderCreateDto;
  boxes: Box[] = [];
  addedBoxes: Record<string, number> = {};
  activeTab: string;

  // Data validation
  amountFormControl = new FormControl(1, [Validators.required, Validators.min(1)]);

  customerForm = new FormGroup({
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phoneNumber: new FormControl('', [Validators.required]),
  });

  addressForm = new FormGroup({
    streetName: new FormControl('', [Validators.required]),
    houseNumber: new FormControl(0, [Validators.required, Validators.min(1)]),
    houseNumberAddition: new FormControl(''),
    postalCode: new FormControl('', [Validators.required]),
    city: new FormControl('', [Validators.required]),
    country: new FormControl('', [Validators.required]),
  });

  constructor(public boxService: BoxService, public orderService: OrderService) {
    this.boxService.get(1).then(boxes => this.boxes = this.boxService.boxes);
    this.order = {
      boxes: {},
      customer: { simpsonImgUrl: "" }
    };
    this.activeTab = "box-tab";
  }

  changeTab(tabId: string): void {
    this.activeTab = tabId;
  }

  addBox(boxId: string, boxAmount: string, event: Event): void {
    event.stopPropagation();
    this.addedBoxes[boxId] = Number(boxAmount);
    this.order.boxes = this.addedBoxes;
  }

  removeBox(boxId: string, event: Event): void {
    event.stopPropagation();
    delete this.addedBoxes[boxId]; // removes the property from the object
  }

  async onCreateOrder() {
    const address = this.addressForm.value as CreateAddressDto;
    const customer = this.customerForm.value as CreateCustomerDto;
    customer.address = address;
    this.order.customer = customer;
    await this.orderService.create(this.order);
    this.addressForm.reset();
    this.customerForm.reset();
    this.addedBoxes = {};
  }

  protected readonly parseInt = Number.parseInt;
  protected readonly Object = Object;
}
