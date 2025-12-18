import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Box, BoxUpdateDto} from "../../interfaces/box-inteface";
import { BoxService } from "../../services/box-service";

@Component({
  selector: 'app-box-list',
  templateUrl: './list.component.html',
})
export class ListComponent {
  @Input() boxes: Box[] = [];
  @Input() currentPage: number = 1;
  @Input() totalPages: number = 1;

  @Output() editBoxPressed = new EventEmitter<string>();
  @Output() deleteBoxPressed = new EventEmitter<string>();
  @Output() pageChanged = new EventEmitter<number>();

  onDeleteBox(boxId: string) {
    this.deleteBoxPressed.emit(boxId);
  }

  onEditBox(boxId: string) {
    this.editBoxPressed.emit(boxId);
  }

  onPageChanged(page: number) {
    this.pageChanged.emit(page);
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    for (let i = 1; i <= this.totalPages; i++) {
      pages.push(i);
    }
    return pages;
  }
}
