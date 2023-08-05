import { Component, Input, OnInit } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import * as funnelPlugin from 'chartjs-plugin-funnel';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-funnel-graph',
  templateUrl: './funnel-graph.component.html',
  styleUrls: ['./funnel-graph.component.scss']
})
export class FunnelGraphComponent implements OnInit {
  @Input() height: number;
  @Input() id: string; 
  @Input() data: any;
  @Input() options: any;

  chart: any;
  constructor() {
    BaseChartDirective.registerPlugin(funnelPlugin);
   }

  ngOnInit() {
    setTimeout(() => {
      this.createFunnelGraph();
    },500)
  }

  createFunnelGraph() {
    if (this.chart)
      this.chart.destroy();
    this.chart = new Chart(this.id, {
              type: 'funnel',
              data: this.data,
              options: this.options
      })
  }

   reload (data : any) {
       this.data = data;
       this.createFunnelGraph();
   }

}
