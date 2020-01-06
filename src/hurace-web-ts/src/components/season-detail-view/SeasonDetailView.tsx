import React from 'react';
import styled from 'styled-components';
import { DetailViewWrapper } from '../shared/DetailViewWrapper';
import { SeasonDetailPanel } from './SeasonDetailPanel';
import { DisciplineView } from './discipline-view/DisciplineView';

const SeasonPanel = styled.div`
    display: flex;
    flex-direction: column;
    overflow: hidden;
`;

const SeasonDetailWrapper = styled.div`
    display: flex;
`;

export const SeasonDetailView: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    return (
        <DetailViewWrapper
            url="/season"
            backText="Zurück zur Saisonübersicht"
            editConfig={{
                editText: 'Saison bearbeiten',
                editUrl: `/season/${seasonId}/update`
            }}
        >
            <SeasonPanel>
                <SeasonDetailWrapper>
                    <SeasonDetailPanel seasonId={seasonId} />
                </SeasonDetailWrapper>
                <DisciplineView seasonId={seasonId} />
            </SeasonPanel>
        </DetailViewWrapper>
    );
};
