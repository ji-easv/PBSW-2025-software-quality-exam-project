import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Order, OrderCreateDto, ShippingStatus } from "../interfaces/order-interface";
import {environment} from "../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  orders: Order[] = [];
  private readonly orderApiUrl = `${environment.apiUrl}/Order`;
  private readonly statsApiUrl = `${environment.apiUrl}/Stats`;

  constructor(private readonly http: HttpClient) {
    this.initializeData();
  }

  private initializeData(): void {
    // Fire-and-forget: start the async operation without awaiting
    this.get().catch(error => {
      console.error('Failed to preload orders:', error);
    });
  }

  async get(): Promise<Order[]> {
    this.orders = await this.fetchOrders();
    return this.orders;
  }

  private async fetchOrders(): Promise<Order[]> {
    const call = this.http.get<Order[]>(`${this.orderApiUrl}`);
    const orders = await firstValueFrom<Order[]>(call);
    orders.forEach(o => o.createdAt = new Date(o.createdAt));
    orders.forEach(o => o.updatedAt = new Date(o.updatedAt ?? o.createdAt));
    return orders;
  }

  public async getbyId(id: string): Promise<Order> {
    const order = await firstValueFrom(this.http.get<Order>(`${this.orderApiUrl}/${id}`));
    order.createdAt = new Date(order.createdAt);
    order.updatedAt = new Date(order.updatedAt ?? order.createdAt);
    return order;
  }

  public async create(orderCreateDto: OrderCreateDto) {
    const order = await firstValueFrom(this.http.post<Order>(`${this.orderApiUrl}`, orderCreateDto));
    order.createdAt = new Date(order.createdAt);
    order.updatedAt = new Date(order.updatedAt ?? order.createdAt);
    return order;
  }

  public updateStatus(id: string, status: ShippingStatus) {
    return firstValueFrom(this.http.patch<Order>(`${this.orderApiUrl}/${id}?newStatus=${status}`, null));
  }
  public delete(id: string) {
    return firstValueFrom(this.http.delete(`${this.orderApiUrl}/${id}`));
  }

  public async getLatest(): Promise<Order[]> {
    const call = this.http.get<Order[]>(`${this.orderApiUrl}/latest`);
    const orders = await firstValueFrom(call);
    orders.forEach(o => o.createdAt = new Date(o.createdAt));
    orders.forEach(o => o.updatedAt = new Date(o.updatedAt ?? o.createdAt));
    return orders;
  }

  public async getTotalRevenue(): Promise<number> {
    const call = this.http.get<number>(`${this.orderApiUrl}/revenue`);
    return await firstValueFrom(call);
  }

  public async getBoxesSold(): Promise<number> {
    const call = this.http.get<number>(`${this.orderApiUrl}/boxes-sold`);
    return await firstValueFrom(call);
  }

  public async getOrdersCount(): Promise<number> {
    const call = this.http.get<number>(`${this.orderApiUrl}/orders-count`);
    return await firstValueFrom(call);
  }

  public async getOrdersCountByMonth(): Promise<Map<number, number>> {
    const call = this.http.get<{ [key: number]: number }>(this.statsApiUrl);
    return new Map(Object.entries(await firstValueFrom(call)).map(([key, value]) => [Number.parseInt(key), value]));
  }
}
