import { MusicianResponse } from "../../musician/interfaces";
import { SelectItem } from "../../shared/interfaces";

export interface MostUsedInstrumentResponse{
  musiciansByInstrumentName: Record<string, MusicianResponse[]>;
}
