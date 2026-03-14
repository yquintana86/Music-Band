import { InstrumentType } from "./instrument-type.enum";

export interface UpdateInstrumentCommand {
  id: number;
  name: string;
  country?: string;
  description?: string;
  musicianId: number;
  type: InstrumentType
}
