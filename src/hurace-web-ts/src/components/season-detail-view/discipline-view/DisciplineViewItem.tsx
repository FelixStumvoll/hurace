import React from 'react';
import styled from 'styled-components';
import { DisciplineData } from '../../../interfaces/DisciplineData';
import { RaceViewItem } from './RaceViewItem';
import { DefaultLink } from '../../../theme/StyledComponents';
import { HeaderCard } from '../../shared/HeaderCard';

const PanelWrapper = styled.div`
    margin-bottom: 10px;
`;

const RacesPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
`;

export const DisciplineViewItem: React.FC<{
    disciplineData: DisciplineData;
    seasonId: number;
}> = ({ disciplineData, seasonId }) => {
    return (
        <PanelWrapper>
            <HeaderCard headerText={disciplineData.discipline.disciplineName}>
                <RacesPanel>
                    {disciplineData.races.map(r => (
                        <DefaultLink
                            key={r.id}
                            to={`/season/${seasonId}/race/${r.id}`}
                        >
                            <RaceViewItem race={r} />
                        </DefaultLink>
                    ))}
                </RacesPanel>
            </HeaderCard>
        </PanelWrapper>
    );
};
