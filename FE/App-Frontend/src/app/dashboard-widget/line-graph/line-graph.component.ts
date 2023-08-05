import { Component, Input, OnInit } from '@angular/core';
import { Chart } from 'chart.js';

@Component({
  selector: 'cdk-line-graph',
  templateUrl: './line-graph.component.html',
  styleUrls: ['./line-graph.component.scss']
})
export class LineGraphComponent implements OnInit {
  @Input() height: number;
  @Input() id: string;
  @Input() data: any;
  @Input() options: any;
  chart: any;
  constructor() { }

  ngOnInit() {
    setTimeout(() => {
        this.createLineChart();
    },1000)
  }
  createLineChart() {
    if (this.chart)
         this.chart.destroy();
    this.chart = new Chart(this.id, {
                type: 'line',
                data: this.data,
                options: this.options
        })
   } 

   reload (data : any) {
       this.data = data;
       this.createLineChart();
   }

}
