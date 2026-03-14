import { InstrumentType } from ".";

export interface CreateInstrumentCommand {
  name: string;
  country?: string;
  description?: string;
  musicianId: number;
  type: InstrumentType;
}
