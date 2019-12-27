import React from 'react';
import { Season } from '../../interfaces/Season';
import { ItemCard } from '../../theme/StyledComponents';

export const SeasonViewItem: React.FC<{ season: Season }> = ({ season }) => {
    return (
        <ItemCard>
            {season.startDate.getFullYear()} <b>-</b>{' '}
            {season.endDate.getFullYear()}
        </ItemCard>
    );
};
