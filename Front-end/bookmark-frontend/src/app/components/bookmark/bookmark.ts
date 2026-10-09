import { Component, inject, OnInit } from '@angular/core';
import { BookmarkService } from '../../services/BookmarkService';

@Component({
  selector: 'app-bookmark',
  templateUrl: './bookmark.html',
})
export class Bookmark implements OnInit {
  service = inject(BookmarkService);

  ngOnInit() {
    this.service.load();
  }

  remove(id: number) {
    this.service.delete(id).subscribe();
  }
}