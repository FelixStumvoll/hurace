import React from 'react';
import { ItemCard } from '../../theme/StyledComponents';
import { Location } from '../../interfaces/Location';

export const LocationViewItem: React.FC<{ location: Location }> = ({
    location
}) => {
    return <ItemCard>{location.locationName}</ItemCard>;
};
