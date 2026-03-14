import { InstrumentType } from "../../instrument/interfaces";

export interface AverageByInstrumentsQuery {
  instrumentIds: number[];
  instrumentType: InstrumentType;
}
