import { Component } from '@angular/core';
import { ShippingStatus } from "../../interfaces/order-interface";
import { OrderService } from "../../services/order-service";

@Component({
  selector: 'app-order-list',
  templateUrl: './list.component.html',
})
export class ListComponent {

  constructor(public orderService: OrderService) {
  }

  protected readonly Object = Object;
  protected readonly ShippingStatus = ShippingStatus;

  changeStatus(id: string, status: ShippingStatus) {
    this.orderService.updateStatus(id, status).then(r => {
      this.orderService.get();
    });
  }
}
