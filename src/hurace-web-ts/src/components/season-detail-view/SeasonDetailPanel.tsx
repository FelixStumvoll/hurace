import React from 'react';
import { HeaderCard } from '../shared/HeaderCard';
import styled from 'styled-components';
import { getSeasonById } from '../../common/api';
import { getDate } from '../../common/timeConverter';
import { useStateAsync } from '../../hooks/useStateAsync';

const DetailGrid = styled.div`
    display: grid;
    grid-template-columns: auto auto;
    grid-template-rows: auto auto;
    column-gap: 10px;
    row-gap: 5px;
`;

const SeasonLabel = styled.span`
    font-weight: bold;
`;

export const SeasonDetailPanel: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const [season] = useStateAsync(getSeasonById, seasonId);

    return (
        <HeaderCard headerText="Stammdaten">
            {season && (
                <DetailGrid>
                    <SeasonLabel>Startdatum:</SeasonLabel>
                    <span>{getDate(season.startDate)}</span>
                    <SeasonLabel>Enddatum:</SeasonLabel>
                    <span>{getDate(season.endDate)}</span>
                </DetailGrid>
            )}
        </HeaderCard>
    );
};
