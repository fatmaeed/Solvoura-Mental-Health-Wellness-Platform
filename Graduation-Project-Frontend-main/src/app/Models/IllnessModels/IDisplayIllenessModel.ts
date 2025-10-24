interface IDisplayIllnessModel {
    id:                 number;
    illnessName:        string;
    illnessDescription: string;
    level:              string;
    image:              string;
    relatedSpecialties: string[];
    symptoms:           IDisplaySymptomModel[];
      
}