import React from 'react';
import { Season } from '../../models/Season';
import { Card, DefaultLink } from '../../theme/CustomComponents';
import styled from 'styled-components';

const SeasonItem = styled(Card)`
    margin: 0 10px 10px 0;
    display: flex;
    flex-direction: column;
`;

export const SeasonListViewItem: React.FC<{ season: Season }> = ({
    season
}) => {
    return (
        <DefaultLink to={`season/${season.id}`}>
            <SeasonItem>
                <span>
                    {season.startDate.getFullYear()} <b>-</b>{' '}
                    {season.endDate.getFullYear()}
                </span>
            </SeasonItem>
        </DefaultLink>
    );
};
