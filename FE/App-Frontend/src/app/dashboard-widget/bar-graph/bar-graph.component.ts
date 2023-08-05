import { Component, Input, OnInit } from '@angular/core';
import { Chart } from 'chart.js';


@Component({
  selector: 'cdk-bar-graph',
  templateUrl: './bar-graph.component.html',
  styleUrls: ['./bar-graph.component.scss']
})
export class BarGraphComponent implements OnInit {
  @Input() height: number;
  @Input() id: string;
  @Input() data: any;
  @Input() options: any;
  constructor() { }
  
  chart: any;
  ngOnInit() {
    setTimeout(() => {
      this.createBarGraph();
    },500)
  }

  createBarGraph() {
    if (this.chart)
      this.chart.destroy();
    this.chart = new Chart(this.id, {
              type: 'bar',
              data: this.data,
              options: this.options
      })
  }

   reload (data : any) {
       this.data = data;
       this.createBarGraph();
   }

}
