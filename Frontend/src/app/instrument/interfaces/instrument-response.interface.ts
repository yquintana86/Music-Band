import { InstrumentType } from "./instrument-type.enum";

export interface InstrumentResponse {
  id: number;
  name: string;
  country?: string;
  description?: string;
  type: InstrumentType;
  musicianId: number;
  musicianName: string;
}
