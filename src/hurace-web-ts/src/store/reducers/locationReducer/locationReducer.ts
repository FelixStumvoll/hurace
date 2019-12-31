import { Location } from '../../../interfaces/Location';
import { LocationAction } from './locationActions';

export interface LocationReducerState {
    locations: Location[];
}

const initialState: LocationReducerState = {
    locations: [
        // { id: 1, locationName: 'Kitzbühl', cou },
        // { id: 2, locationName: 'Sölden' },
        // { id: 3, locationName: 'IDK' },
        // { id: 4, locationName: 'Kitzbühl' },
        // { id: 5, locationName: 'Kitzbühl' }
    ]
};

export const locationReducer = (
    state = initialState,
    action: LocationAction
) => {
    return state;
};
