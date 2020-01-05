import React from 'react';
import { Season } from '../../interfaces/Season';
import { Card, DefaultLink } from '../../theme/StyledComponents';
import styled from 'styled-components';

const SeasonItem = styled(Card)`
    margin-bottom: 10px;
    display: flex;
    flex-direction: column;
`;

export const SeasonViewItem: React.FC<{ season: Season }> = ({ season }) => {
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
