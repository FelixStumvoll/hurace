import React from 'react';
import { HeaderCard } from '../shared/HeaderCard';
import styled from 'styled-components';
import { getSeasonById } from '../../common/api';
import { getDate } from '../../common/timeConverter';
import { TextBold } from '../../theme/CustomComponents';
import { LoadingWrapper } from '../shared/LoadingWrapper';
import { useAsync } from 'react-async-hook';

const DetailGrid = styled.div`
    display: grid;
    grid-template-columns: auto auto;
    grid-template-rows: auto auto;
    column-gap: 10px;
    row-gap: 5px;
`;

export const SeasonDetailPanel: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const { loading, error, result: season } = useAsync(getSeasonById, [
        seasonId
    ]);

    return (
        <HeaderCard headerText="Stammdaten">
            <LoadingWrapper loading={loading} error={error}>
                {season && (
                    <DetailGrid>
                        <TextBold>Startdatum:</TextBold>
                        <span>{getDate(season.startDate)}</span>
                        <TextBold>Enddatum:</TextBold>
                        <span>{getDate(season.endDate)}</span>
                    </DetailGrid>
                )}
            </LoadingWrapper>
        </HeaderCard>
    );
};
